using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Theatre.Models;

namespace Theatre.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly DatabaseContext _context;

        public EmployeeController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index(int role)
        {
            ViewData["PostID"] = new SelectList(_context.Posts, "ID", "Name");
            ViewData["CurrentRole"] = role;
            EmployeeIndex employeeIndex = new EmployeeIndex
            {
                Employees = _context.Employees.Include(e => e.Post),
                CurrentRole = role
            };
            return View(employeeIndex);
        }


        [HttpPost]
        public async Task<IActionResult> Create(EmployeeIndex employeeIndex)
        {
            Employee employee = employeeIndex.Employee;
            ViewData["CurrentRole"] = employeeIndex.CurrentRole;
            bool LoginExists = _context.Employees.Any
                (x => x.Login == employeeIndex.Employee.Login && x.ID != employeeIndex.Employee.ID && employeeIndex.Employee.Login != "_");

            if(LoginExists)
            {
                ModelState.AddModelError("Login", "");
                ViewData["LoginExists"] = "Сотрудник с таким логином уже существует";
            }

            if (ModelState.IsValid)
            {
                string FullName = $"{employee.LastName} {employee.FirstName} {employee.MiddleName}";
                employeeIndex.Employee.FullName = FullName;
                _context.Add(employeeIndex.Employee);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index), employeeIndex);
            }
            ViewData["PostID"] = new SelectList(_context.Posts, "ID", "Name");
            employeeIndex = new EmployeeIndex { Employees = _context.Employees.ToList() };
            return View(nameof(Index), employeeIndex);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            
            ViewData["PostID"] = new SelectList(_context.Posts, "ID", "Name", employee.PostID);
            return View(employee);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (id != employee.ID)
            {
                return NotFound();
            }

            bool LoginExists = _context.Employees.Any
               (x => x.Login == employee.Login && x.ID != employee.ID && employee.Login != "_");

            if (LoginExists)
            {
                ModelState.AddModelError("Login", "");
                ViewData["LoginExists"] = "Сотрудник с таким логином уже существует";
            }

            ViewData["PostID"] = new SelectList(_context.Posts, "ID", "Name", employee.PostID);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                EmployeeIndex employeeIndex = new EmployeeIndex() { Employees = _context.Employees.Include(e => e.Post)};
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "Index", employeeIndex) });
            }
            
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", employee) });
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Post)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (employee == null)
            {
                return NotFound();
            }
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.ID == id);
        }
    }
}
