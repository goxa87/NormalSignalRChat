using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRLessons.Models.DBO
{
    public class Chat
    {
        [Key]
        public int ChatId { get; set; }

        public bool IsPublicChat { get; set; }
        public List<Message> Messages { get; set; }
        public List<UserChat> UserChats { get; set; }
    }
}
