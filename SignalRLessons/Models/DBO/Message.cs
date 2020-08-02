using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRLessons.Models.DBO
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        
        public DateTime MessageDate { get; set; }
        [MaxLength(1024)]
        public string Text { get; set; }

        public bool IsRead { get; set; }

        public string  SenderId { get; set; }

        public string SenderName { get; set; }

        public int ChatId { get; set; }
    }
}
