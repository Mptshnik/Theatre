using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Theatre.Models
{
    public class Membership
    {
        [Key] public int ID { get; set; }

        [Required(ErrorMessage = "Поле обязательно")]
        [DisplayName("Стоимость абонемента (₽)")]
        [Range(0, 100000, ErrorMessage = "Значение должно быть в пределе от 0 до 100000")]
        public double Price { get; set; }

        [DisplayName("Жанр")]
        public string Genre { get; set; }
        [DisplayName("Автор")]
        [AllowNull]
        public int? AuthorID { get; set; }
        [DisplayName("Автор")]
        public Author Author { get; set; }
    }
}
