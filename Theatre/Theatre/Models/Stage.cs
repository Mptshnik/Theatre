using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Theatre.Models
{
    public class Stage
    {
        [Key]
        public int ID { get; set; }
        [DisplayName("Сотрудник")]
        public Employee Employee { get; set; }
        [DisplayName("Сотрудник")]
        [Required(ErrorMessage = "Сотрудник обязателен")]
        public int EmployeeID { get; set; }
        [DisplayName("Спектакль")]
        public Performance Performance { get; set; }
        [DisplayName("Спектакль")]
        [Required(ErrorMessage = "Спектакль обязателен")]
        public int PerformanceID { get; set; }
    }
}
