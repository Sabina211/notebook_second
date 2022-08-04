using DataAnnotationsExtensions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiNotebook.Models
{
    public class UserWithRolesEdit 
    {
        [Required(ErrorMessage = "Необходимо заполнить поле Id")]
        public Guid Id { get; set; }
        public List<IdentityRole> AllRoles { get; set; }
        public IList<string> UserRoles { get; set; }

        [Email]
        public string Email { get; set; }

        [Required(ErrorMessage = "Необходимо заполнить поле Логин")]
        public string UserName { get; set; }


        public UserWithRolesEdit()
        {
            AllRoles = new List<IdentityRole>();
            UserRoles = new List<string>();
        }
    }
}
