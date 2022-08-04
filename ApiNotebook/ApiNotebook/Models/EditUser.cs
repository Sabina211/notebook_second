using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiNotebook.Models
{
    public class EditUser
    {
        [Required(ErrorMessage = "Необходимо указать Id")]
        public string Id { get; set; }

        public string Login { get; set; }

        [Email]
        public string Email { get; set; }
    }
}
