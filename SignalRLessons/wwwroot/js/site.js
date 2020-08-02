$(document).ready(function () {
    const hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("/Chat/Chat")
        .build();

    hubConnection.on("send", function (message, user) {
        let container = $('.message-list')
        let item = `<div class="message"><div>${user}</div><div>${message}</div></div>`;
        container.prepend(item);
    })

    function callNewMessages() {
        let chatId = $('#current-user-chat-id').val();
        hubConnection.invoke("CallNewMessages", chatId);
    }

    function callAllMessages() {
        let chatId = $('#current-user-chat-id').val();
        hubConnection.invoke("CallAllMessages", chatId);
    }

    function renderMessages(messages) {
        console.log('render messages', messages)
        $(messages).each((i,mes) => {
            let block = `<div class="message">
                            <div><sub>${mes.senderName}</sub></div>
                            <div class="message-text">${mes.text}</div>
                            <div><sub>${mes.messageDate}</sub></div>
                        </div>`
            $('.message-list').prepend(block);
        })
    }

    hubConnection.on('getMessageCount', function (_chatId) {
        console.log( 'chat, ', _chatId )
        let opponents = $('.opponent')
        let opponent = '!';
        opponents.each(function (i, e) {
            if ($(e).children('.opponent-chat-id').val() == _chatId) {
                opponent = e;              
            }
        })
        let counter = $(opponent).children('.opponent-message-count');
        if ($(counter).text !== '') {
            counter.text(Number.parseInt($(counter).text()) + 1);
        } else {
            counter.text('1');
        }
        // Если этот чат открыт, получить сообщения
        if ($('#current-user-chat-id').val() == _chatId) {
            callNewMessages();
            $(opponent).children('.opponent-message-count').text('');
        }
    })
    
    hubConnection.on('getMessages', function (messages) {
        renderMessages(messages);
    })

    //getAllMessages

    hubConnection.on('getAllMessages', function (messages) {
        $('.message-list').html('');
        renderMessages(messages);
    })

    $('#send-message').on('click', function () {
        let chatId = $('#current-user-chat-id').val();
        console.log(chatId)
        if (chatId !== '') {
            let message = $('#message').val();
            $('#message').val('');
            let chatId = $('#current-user-chat-id').val();
            let _M1 = [{ senderName: $('#current-user-name').val(), text: message, messageDate: new Date() }]
            renderMessages(_M1);
            console.log('message', message, 'chatId', chatId)
            hubConnection.invoke("SendMessage", chatId, message);
        }
    })

    hubConnection.start();

    $('.opponent').click(function () {
        $('#current-user-chat-id').val($(this).children('.opponent-chat-id').val());
        $('.message-list').html('');
        $(this).children('.opponent-message-count').html('');
        $('#header-title').text($(this).children('.opponent-name').text())
        callAllMessages()
    })
})