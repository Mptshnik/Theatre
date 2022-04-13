using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Theatre.Models
{
    public class Ticket
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "Стоимость обязательна")]
        [Range(0, 100000, ErrorMessage = "Стоимость должна быть в диапазоне от 0 до 100000")]
        [DisplayName("Стоимость билета (₽)")]
        public double Price { get; set; }
        [DisplayName("Номер места")]
        [Required(ErrorMessage = "Номер места обязателен")]
        [RegularExpression("[0-9]+", ErrorMessage = "Номер места должен состоять из цифр 0-9")]
        [Range(0, 5000, ErrorMessage = "Номер места должен быть в диапазоне от 0 до 5000")]
        public int SeatNumber { get; set; }
        [DisplayName("Спектакль")]
        [AllowNull]
        public int? PerformanceID { get; set; }
        [DisplayName("Спектакль")]
        [AllowNull]
        public Performance Performance { get; set; }
    }
}
