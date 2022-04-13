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
    public class StageController : Controller
    {
        private readonly DatabaseContext _context;

        public StageController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index(int role)
        {
            ViewData["CurrentRole"] = role;
            ViewData["EmployeeID"] = new SelectList(_context.Employees.Where(e => e.Role != (int)Roles.Admin), "ID", "FullName");
            ViewData["PerformanceID"] = new SelectList(_context.Performances, "ID", "Name");
            var databaseContext = _context.Stages.Include(s => s.Employee).Include(s => s.Performance).Include(e => e.Employee.Post);
            StageIndex stageIndex = new StageIndex {
                Stages = databaseContext.ToList(),
                CurrentRole = role
            };
            return View(stageIndex);
        }
     

        [HttpPost]
        public async Task<IActionResult> Create(StageIndex stageIndex)
        {
            Stage stage = stageIndex.Stage;

           
            ViewData["CurrentRole"] = stageIndex.CurrentRole;
            ViewData["EmployeeID"] = new SelectList(_context.Employees.Where(e => e.Role != (int)Roles.Admin), "ID", "FullName", stage.EmployeeID);
            ViewData["PerformanceID"] = new SelectList(_context.Performances, "ID", "Name", stage.PerformanceID);
     
            if (ModelState.IsValid)
            {
                _context.Add(stage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
           
            stageIndex.Stages = _context.Stages.Include(s => s.Employee).Include(s => s.Performance).Include(e => e.Employee.Post);
            return View(nameof(Index), stageIndex);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stage = await _context.Stages.FindAsync(id);

            if (stage == null)
            {
                return NotFound();
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employees.Where(e => e.Role != (int)Roles.Admin), "ID", "FullName", stage.EmployeeID);
            ViewData["PerformanceID"] = new SelectList(_context.Performances, "ID", "Name", stage.PerformanceID);
            return View(stage);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id,  Stage stage)
        {
            if (id != stage.ID)
            {
                return NotFound();
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employees.Where(e => e.Role != (int)Roles.Admin), "ID", "FullName", stage.EmployeeID);
            ViewData["PerformanceID"] = new SelectList(_context.Performances, "ID", "Name", stage.PerformanceID);

           
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StageExists(stage.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                StageIndex stageIndex = new StageIndex { Stages = _context.Stages.Include(s => s.Employee).Include(s => s.Performance)
                    .Include(e => e.Employee.Post)};
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "Index", stageIndex) });
            }

            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", stage) });
        }

     
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stage = await _context.Stages
                .Include(s => s.Employee)
                .Include(s => s.Performance)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (stage == null)
            {
                return NotFound();
            }

            _context.Stages.Remove(stage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

      
        private bool StageExists(int id)
        {
            return _context.Stages.Any(e => e.ID == id);
        }
    }
}
