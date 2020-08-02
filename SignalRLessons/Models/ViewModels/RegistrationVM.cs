using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRLessons.Models.ViewModels
{
    public class RegistrationVM
    {
        [Display(Name = "Адрес электронной почты")]
        [Required(ErrorMessage = "Не введен адрес")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Неверный формат электронной почты")]
        public string Email { get; set; }

        [Display(Name = "Придумате пароль не менее 6 символов")]
        [Required(ErrorMessage = "Не введен пароль.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Введите пароль еще раз")]
        [Required(ErrorMessage = "Повторите пароль")]
        [DataType(DataType.Password), Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
        public string СonfirmPassword { get; set; }

        /// <summary>
        /// Псевдоним
        /// </summary>
        [Display(Name = "Ваше имя(будет видно другим пользователям)")]
        [Required(ErrorMessage = "Введите псевдоним"), MaxLength(200, ErrorMessage = "Максимальная длина 200 символов")]
        public string Name { get; set; }

        /// <summary>
        /// Фото на аватарку.
        /// </summary>
        [Display(Name = "Ваша форография")]
        public IFormFile Photo { get; set; }
    }
}
