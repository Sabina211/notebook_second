using DataAnnotationsExtensions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiNotebook.Models
{
    public class UserWithRolesAdd 
    {
        public List<IdentityRole> AllRoles { get; set; }
        public IList<string> UserRoles { get; set; }

        [Email(ErrorMessage ="Некорректный формат email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Необходимо заполнить поле Логин")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Необходимо заполнить поле Пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        [StringLength(100, ErrorMessage = "Поле {0} должно содержать не менее 4 символов и не более 100", MinimumLength = 4)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Required(ErrorMessage = "Необходимо заполнить поле Подтверждение пароля")]
        public string ConfirmPassword { get; set; }

        public UserWithRolesAdd()
        {
            AllRoles = new List<IdentityRole>();
            UserRoles = new List<string>();
        }
    }
}
