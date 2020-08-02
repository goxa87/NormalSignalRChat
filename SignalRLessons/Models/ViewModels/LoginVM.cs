using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRLessons.Models.ViewModels
{    
    public class LoginVM
    {
        [Required(ErrorMessage = "Не заполнено поле \"Логин\"")]
        [Display(Name = "Логин(электронная почта):")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Не заполнено поле \"Пароль\"")]
        [Display(Name = "Пароль:")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}