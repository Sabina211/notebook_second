using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NotebookSecond.Entities
{
    public class Worker
    {
        public Guid Id { get; set; }//поле айди обязательно для энтити

        [Required(ErrorMessage = "Необходимо заполнить имя сотрудника"),
         StringLength(200, ErrorMessage ="Поле должно содержать не больше 200 символов")]
        public string Name { get; set; }

        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
    }
}
