using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRLessons.Models.DBO
{
    /// <summary>
    /// Пользователь системы
    /// </summary>
    public class User : IdentityUser
    {
        /// <summary>
        /// имя для отображения
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// статус пользователя
        /// </summary>
        public int Status { get; set; }

        public string Photo{ get; set; }

        public List<UserChat> UserChats { get; set; }
    }
}
