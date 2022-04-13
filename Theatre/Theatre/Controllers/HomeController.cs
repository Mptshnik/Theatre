using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Theatre.Models;

namespace Theatre.Controllers
{
    public class HomeController : Controller
    {
        private DatabaseContext _context;

        public HomeController(DatabaseContext context)
        {
            _context = context;

        }

        public IActionResult Index()
        {
            List<Post> posts = new List<Post>()
            {
                new Post(){ Name = "Администратор" }
            };

            if(_context.Posts.Where(p => p.Name == "Администратор").FirstOrDefault() == null)
            {
                _context.Posts.AddRange(posts);
                _context.SaveChanges();
            }


            Employee employee = new Employee()
            {
                FirstName = "Иван",
                LastName = "Иванов",
                Gender = "_",
                Login = "admin",
                Post = _context.Posts.Find(1),
                Password = "admin",
                Role = (int)Roles.Admin
            };

            if (_context.Employees.Where(p => p.Login == "admin").FirstOrDefault() == null)
            {
                _context.Employees.Add(employee);
                _context.SaveChanges();
            }

            return View();
        }

        public IActionResult AdminPage()
        {
            return View();
        }

        public IActionResult PerformanceDirectorPage()
        {
            return View();
        }


        public IActionResult TheatreDirectorPage()
        {
            return View();
        }


        public IActionResult CashierPage()
        {
            return View();
        }



        public IActionResult Authorize(string login, string password)
        {
            ViewBag.CurrentUserRole = 10;
            if (login == null)
            {
                ViewData["InvalidLogin"] = "Поле логина не заполнено";
            }
            else
            {
                Employee employee = _context.Employees.Where(e => e.Login == login).FirstOrDefault();
                if (employee != null)
                {
                    if (employee.Password == password)
                    {
                        if (employee.Role == (int)Roles.Admin)
                        {
                           return View("AdminPage");
                        }
                        else if(employee.Role == (int)Roles.PerformanceDirector)
                        {
                            return View("PerformanceDirectorPage");
                        }
                        else if(employee.Role == (int)Roles.Cashier)
                        {
                            return View("CashierPage");
                        }
                        else if(employee.Role == (int)Roles.TheatreDirector)
                        {
                            return View("TheatreDirectorPage");
                        }
                        else
                        {
                            ViewData["InvalidLogin"] = "Что-то пошло не так";
                        }

                    }
                    else
                    {
                        ViewData["InvalidPassword"] = "Не верный пароль";
                    }
                }
                else
                {
                    ViewData["InvalidLogin"] = "Пользователя с таким логином не существует";
                }
            }

            if(password == null)
            {
                ViewData["InvalidPassword"] = "Поле пароля не заполнено";
            }
         

            return View(nameof(Index));
        }
    }
}
