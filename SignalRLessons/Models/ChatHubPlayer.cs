using SignalRLessons.Models.DBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRLessons.Models
{
    public class ChatHubPlayer
    {

        public string HubConnectionId { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }
    }
}
