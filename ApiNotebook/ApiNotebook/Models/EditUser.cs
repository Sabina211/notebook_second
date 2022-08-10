using DataAnnotationsExtensions;
using System.ComponentModel.DataAnnotations;

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
