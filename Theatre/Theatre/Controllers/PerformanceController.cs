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
    public class PerformanceController : Controller
    {
        private readonly DatabaseContext _context;

        public PerformanceController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index(int role)
        {
            ViewData["AuthorID"] = new SelectList(_context.Set<Author>(), "ID", "FullName");
            ViewData["CurrentRole"] = role;
            var databaseContext = _context.Performances.Include(p => p.Author);
            PerformanceIndex performanceIndex = new PerformanceIndex { Performances = databaseContext.ToList(), CurrentRole = role};
            return View(performanceIndex);
        }

     

        [HttpPost]
        public async Task<IActionResult> Create(PerformanceIndex performanceIndex)
        {
            ViewData["CurrentRole"] = performanceIndex.CurrentRole;
            if (ModelState.IsValid)
            {
                _context.Performances.Add(performanceIndex.Performance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorID"] = new SelectList(_context.Set<Author>(), "ID", "FullName", performanceIndex.Performance.AuthorID);          
            performanceIndex = new PerformanceIndex { Performances = _context.Performances.ToList() };
            return View(nameof(Index), performanceIndex);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var performance = await _context.Performances.FindAsync(id);
            if (performance == null)
            {
                return NotFound();
            }
            ViewData["AuthorID"] = new SelectList(_context.Set<Author>(), "ID", "FullName", performance.AuthorID);
            return View(performance);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, Performance performance)
        {
            if (id != performance.ID)
            {
                return NotFound();
            }
            ViewData["AuthorID"] = new SelectList(_context.Authors, "ID", "FullName", performance.AuthorID);
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(performance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PerformanceExists(performance.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                PerformanceIndex performanceIndex = new PerformanceIndex { Performances = _context.Performances.Include(p => p.Author) };
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "Index", performanceIndex) });
            }           
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", performance) });
        }

        [HttpGet]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var performance = await _context.Performances
                .Include(p => p.Author)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (performance == null)
            {
                return NotFound();
            }

            _context.Performances.Remove(performance);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        private bool PerformanceExists(int id)
        {
            return _context.Performances.Any(e => e.ID == id);
        }
    }
}
