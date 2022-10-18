using DataAnnotationsExtensions;
using Microsoft.AspNetCore.Identity;
using NotebookSecond.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NotebookSecond.Models
{
    public class UserWithRoles : User
    {
        public List<IdentityRole> AllRoles { get; set; }
        public IList<string> UserRoles { get; set; }
        
        [Email]
        override public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Пароль*")]
        [StringLength(100, ErrorMessage = "Поле {0} должно содержать не менее 4 символов и не более 100", MinimumLength = 4)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля*")]
        public string ConfirmPassword { get; set; }

        public UserWithRoles()
        {
            AllRoles = new List<IdentityRole>();
            UserRoles = new List<string>();
        }
    }
}
