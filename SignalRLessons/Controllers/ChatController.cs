using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Schema;
using SignalRLessons.Models.DBO;
using SignalRLessons.Models.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SignalRLessons.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly ChatDbContext context;
        readonly IWebHostEnvironment environment;

        public ChatController(
            UserManager<User> _userManager,
            SignInManager<User> _signInManager,
            ChatDbContext _context,
            IWebHostEnvironment _environment
            )
        {
            userManager = _userManager;
            signInManager = _signInManager;
            context = _context;
            environment = _environment;
        }

        public async Task<IActionResult> Chat(string opponent = null)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var messages = new List<Message>();
            if (opponent != null)
            {
                var oponentUser = await userManager.FindByIdAsync(opponent);
                if(oponentUser != default)
                {
                    var opponentChat = await context.Chats.Include(e => e.UserChats)
                        .FirstOrDefaultAsync(e => e.UserChats.Any(x => x.UserId == opponent)
                            && e.UserChats.Any(x => x.UserId == user.Id));
                    // Если такоогоь чтат нет, то создаем чат и юзер чаты
                    if(opponentChat == default)
                    {
                        var userChat1 = new UserChat() { ChatName = oponentUser.Name, ChatPhoto = oponentUser.Photo, UserId = user.Id };
                        var userChat2 = new UserChat() { ChatName = user.Name, ChatPhoto = user.Photo, UserId = oponentUser.Id };
                        var newChat = new Chat() { UserChats = new List<UserChat> { userChat1, userChat2 } };
                        context.Chats.Add(newChat);
                        await context.SaveChangesAsync();
                        ViewData["CurrentChat"] = newChat.ChatId;
                        ViewData["CurrentChatName"] = oponentUser.Name;
                        
                        
                    }
                    else
                    {
                        ViewData["CurrentChat"] = opponentChat.ChatId;
                        ViewData["CurrentChatName"] = oponentUser.Name;
                        messages = await context.Messages.Where(e => e.ChatId == opponentChat.ChatId).ToListAsync();
                    }

                }
                else
                {
                    ViewData["CurrentChat"] = null;
                }               
            }
            else
            {
                ViewData["CurrentChat"] = null;
            }
            
            var chats = await context.UserChats.Include(e => e.Chat).ThenInclude(e => e.Messages).Where(chat => chat.UserId == user.Id).ToListAsync();

            var model = new ChatVM()
            {
                userChats = chats,
                UserId = user.Id,
                UserName = user.Name,
                messages = messages
            };
            return View(model);
        }
    }
}
