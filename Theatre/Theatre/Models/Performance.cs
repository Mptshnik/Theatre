using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Theatre.Models
{
    public class Performance
    {
        [Key] public int ID { get; set; }
        [DisplayName("Наименование")]
        [Required(ErrorMessage = "Наименование обязательно")]
        [RegularExpression(@"[a-zA-Zа-яА-Я0-9]+", ErrorMessage = "Наименование должно состоять из букв русского или английского алфавита или цифр 0-9")]
        public string Name { get; set; }
        [DisplayName("Дата и время премьеры")]
        [Date(ErrorMessage = "Дата должна быть больше текущей")]
        [Required(ErrorMessage = "Дата обязательна")]
        [DataType(DataType.DateTime, ErrorMessage = "Выберите дату")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-mm-yyy-hh-mm}", ConvertEmptyStringToNull = true)]
        public string PremiereDate { get; set; }
        [DisplayName("Время начала")]
        [Date(ErrorMessage = "Дата должна быть больше текущей")]
        [Required(ErrorMessage = "Время начала обязательно")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-mm-yyy}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.DateTime, ErrorMessage = "Выберите дату")]
        public string BeginningTime { get; set; }
        [DisplayName("Тип спектакля")]
        [Required]
        public string PerformanceType { get; set; }

        [DisplayName("Жанр")]
        [Required]
        public string Genre { get; set; }

        [DisplayName("Автор")]
        [AllowNull]
        public int? AuthorID { get; set; }
        [DisplayName("Автор")]
        public Author Author { get; set; }

        public virtual List<Employee> Employees { get; set; } = new List<Employee>();
        public virtual List<Ticket> Tickets { get; set; } = new List<Ticket>();
    }

    public class DateAttribute : RangeAttribute 
    {
        public DateAttribute() : base(typeof(DateTime),
           DateTime.Now.ToShortDateString(),
           DateTime.Now.AddYears(100).ToShortDateString())
        { }
    }
}
