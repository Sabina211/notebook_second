using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NotebookSecond.Models
{
    public class LoginUser
    {
        [Required(ErrorMessage = "Необходимо заполнить поле Логин")]
        [Display(Name ="Логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Необходимо заполнить поле Пароль")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
