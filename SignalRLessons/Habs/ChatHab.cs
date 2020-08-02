using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SignalRLessons.Models;
using SignalRLessons.Models.DBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRLessons.Habs
{
    public class ChatHab : Hub
    {
        public static List<ChatHubPlayer> players = new List<ChatHubPlayer>();
        public static int Counter = 0;
        private readonly ChatDbContext context;

        public ChatHab(ChatDbContext db)
        {
            context = db;
        }

        public override async Task OnConnectedAsync()
        {
            var newPlayer = new ChatHubPlayer();
            var user = await context.Users.Include(e=>e.UserChats).ThenInclude(e => e.Chat).ThenInclude(e => e.Messages).FirstOrDefaultAsync(e => e.UserName == Context.User.Identity.Name);
            if(user != null)
            {
                newPlayer.User = user;
                newPlayer.UserId = user.Id;
                newPlayer.HubConnectionId = Context.ConnectionId;
                players.Add(newPlayer);
                await base.OnConnectedAsync();
            }            
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            players.Remove(players.FirstOrDefault(e => e.HubConnectionId == Context.ConnectionId));
            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// При получении сообщения отправляем тем у кого есть чаты отправляем количество непрочитаных и id чата
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessage(string _chatId, string message)
        {
            Counter++;
            var chatId = Int32.Parse(_chatId);
            var playersRecivers = players.Where(e =>  e.User.UserChats.Any(x => x.ChatId == chatId) && e.HubConnectionId != Context.ConnectionId).ToList();
            var user = await context.Users.FirstOrDefaultAsync(e => e.UserName == Context.User.Identity.Name);
            var newMessage = new Message
            {
                MessageDate = DateTime.Now,
                Text = message,
                SenderId = user.Id,
                SenderName = user.Name,
                IsRead = false,
                ChatId = chatId
            };
            try
            {
                context.Messages.Add(newMessage);
                await context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ex " + ex.Message);
            }

            var clients = new List<string>();
            foreach (var u in playersRecivers)
            {
                u.User.UserChats.FirstOrDefault(e => e.ChatId == chatId).Chat.Messages.Add(newMessage);
                clients.Add(u.HubConnectionId);
            }
            await Clients.Clients(clients).SendAsync("getMessageCount", chatId);
        }

        public async Task CallNewMessages(string _chatId)
        {
            var chatId = Int32.Parse(_chatId);

            var user = players.FirstOrDefault(e => e.HubConnectionId == Context.ConnectionId);
            var messages = user.User.UserChats.FirstOrDefault(e => e.ChatId == chatId)
                .Chat.Messages.Where(e => e.IsRead == false).ToList();
            foreach(var mes in messages)
            {
                if (user.User.Id != mes.SenderId)
                {
                    mes.IsRead = true;
                }
            }

            await Clients.Client(user.HubConnectionId).SendAsync("getMessages", messages);
        }

        public async Task CallAllMessages(string _chatId)
        {
            var chatId = Int32.Parse(_chatId);

            var user = players.FirstOrDefault(e => e.HubConnectionId == Context.ConnectionId);
            var messages = user.User.UserChats.FirstOrDefault(e => e.ChatId == chatId).Chat.Messages.ToList();
            foreach (var mes in messages)
            {
                mes.IsRead = true;
            }

            await Clients.Client(user.HubConnectionId).SendAsync("getAllMessages", messages);
        }
    }
}
