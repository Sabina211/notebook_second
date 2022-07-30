using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotebookSecond.Models
{
    public class EditUser
    {
        public string Id { get; set; }

        public string Login { get; set; }

        [Email]
        public string Email { get; set; }
    }
}
