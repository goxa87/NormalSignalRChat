using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRLessons.Models.DBO
{
    public class UserChat
    {
        public int UserChatId { get; set; }

        public string ChatName { get; set; }

        public string ChatPhoto { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public int ChatId { get; set; }
        public Chat Chat { get; set; }
    }
}
