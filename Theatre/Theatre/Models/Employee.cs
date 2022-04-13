using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Theatre.Models
{
    public enum Roles 
    {
        Admin,
        Cashier,
        PerformanceDirector,
        TheatreDirector
    }

    public class Employee
    {
        [Key] 
        public int ID { get; set; }
       
        [DisplayName("Имя")]      
        [Required(ErrorMessage ="Имя пользователя обязательно", AllowEmptyStrings = false)]
        [RegularExpression(@"[a-zA-Zа-яА-Я]+", ErrorMessage = "Имя должно состоять из букв русского или английского алфавита")]
        public string FirstName { get; set; }
        [DisplayName("Фамилия")]
        [Required(ErrorMessage = "Фамилия пользователя обязательна", AllowEmptyStrings = false)]
        [RegularExpression(@"[a-zA-Zа-яА-Я]+", ErrorMessage = "Фамилия должна состоять из букв русского или английского алфавита")]
        public string LastName { get; set; }
        [DisplayName("Отчество")]
        [RegularExpression(@"[a-zA-Zа-яА-Я]+", ErrorMessage = "Отчество должно состоять из букв русского или английского алфавита")]
        public string MiddleName { get; set; }
        [DisplayName("Логин")]    
        [MaxLength(20, ErrorMessage = "Максимальная длина логина 20 символов")]
        [MinLength(6, ErrorMessage = "Минимальная длина логина 6 символов")]
        [RegularExpression(@"[a-zA-Z0-9]+", ErrorMessage = "Логин должен состоять из букв английского алфавита или цифр 0-9")]
        public string Login { get; set; }
        [DisplayName("Пароль")]
        [MaxLength(20, ErrorMessage = "Максимальная длина пароля 20 символов")]
        [MinLength(6, ErrorMessage = "Минимальная длина пароля 6 символов")]
        [RegularExpression(@"[a-zA-Z0-9]+", ErrorMessage = "Пароль должен состоять из букв английского алфавита или цифр 0-9")]
        public string Password { get; set; }
        [DisplayName("Роль")]
        [Required(ErrorMessage = "Роль пользователя обязательна")]
        public int Role { get; set; }
        [DisplayName("Должность")]
        [AllowNull]
        public int? PostID { get; set; }
        [DisplayName("Должность")]
        public Post Post { get; set; }
        [DisplayName("Пол")]
        public string Gender { get; set; }

        [DisplayName("Возраст (Лет)")]
        [Range(18, 150, ErrorMessage = "Возраст должен находится в диапазоне от 18 до 150 лет")]
        public int? Age { get; set; }

        [DisplayName("Рост (См)")]
        [Range(50, 300, ErrorMessage = "Рост должен находится в диапазоне от 50 до 300 См")]
        public int? Height { get; set; }
        [DisplayName("Награды и звания")]
        public string Rewards { get; set; }
        [DisplayName("На гастролях")]
        public bool IsOnTour { get; set; }
        [DisplayName("Студент")]
        public bool IsStudent { get; set; }
        [DisplayName("Тип голоса")]
        public string VoiceType { get; set; }
        public string FullName { get; set; }

        public virtual List<Performance> Performances { get; set; }
    }
}
