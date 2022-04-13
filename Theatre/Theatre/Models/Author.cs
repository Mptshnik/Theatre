using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Theatre.Models
{
    public class Author
    {
        [Key] public int ID { get; set; }

        [DisplayName("ФИО автора")]
        [Required(ErrorMessage = "ФИО является обязательным")]
        [RegularExpression(@"[a-zA-Zа-яА-Я\s]+", ErrorMessage = "ФИО должно состоять из букв русского или английского алфавита")]
        public string FullName { get; set; }


        public virtual List<Performance> Performances { get; set; }
        public virtual List<Membership> Memberships { get; set; }
    }
}
