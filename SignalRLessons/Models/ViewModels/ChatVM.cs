using SignalRLessons.Models.DBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRLessons.Models.ViewModels
{
    public class ChatVM
    {
        public List<UserChat> userChats { get; set; }
        public List<Message> messages { get; set; }

        public string UserId  { get; set; }

        public string UserName { get; set; }
    }
}
