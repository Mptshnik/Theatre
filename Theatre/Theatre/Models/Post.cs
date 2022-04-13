
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Theatre.Models
{
    public class Post
    {
        [Key] public int ID { get; set; }
        [DisplayName("Наименование должности")]
        [Required(ErrorMessage = "Наименование обязательно для заполнения")]
        [RegularExpression(@"[a-zA-Zа-яА-Я]+", ErrorMessage = "Наименование должно состоять из букв русского или английского алфавита")]
        [MaxLength(50)]
        public string Name { get; set; }
        public virtual List<Employee> Employees { get; set; }
    }
}
