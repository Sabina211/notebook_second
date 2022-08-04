using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiNotebook.Models
{
    public class RegisterUser
    {
        [Required(ErrorMessage = "Необходимо заполнить поле Логин")]
        [Display(Name = "Логин"), MaxLength(20)]
        public string Login { get; set; }

        [Required(ErrorMessage = "Необходимо заполнить поле Email"), EmailAddress(ErrorMessage = "Указан некорректный email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Необходимо заполнить поле Пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        [StringLength(100, ErrorMessage = "Поле {0} должно содержать не менее 4 символов и не более 100", MinimumLength = 4)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Необходимо заполнить поле Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        public string ConfirmPassword { get; set; }
    }
}
