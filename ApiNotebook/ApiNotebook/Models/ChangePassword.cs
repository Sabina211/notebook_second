using System.ComponentModel.DataAnnotations;

namespace ApiNotebook.Models
{
    public class ChangePassword
    {
        [Required(ErrorMessage = "Необходимо указать Id")]
        public string Id { get; set; }

        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Поле {0} должно содержать не менее 4 символов и не более 100", MinimumLength = 4)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
    }
}
