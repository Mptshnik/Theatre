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
    public class AuthorController : Controller
    {
        private readonly DatabaseContext _context;

        public AuthorController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index(int role)
        {
            ViewData["CurrentRole"] = role;
            AuthorIndex authorIndex = new AuthorIndex { Authors = _context.Authors.ToList(),
            CurrentRole = role};
            return View(authorIndex);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AuthorIndex authorIndex)
        {
            ViewData["CurrentRole"] = authorIndex.CurrentRole;
            if (ModelState.IsValid)
            {
                _context.Add(authorIndex.Author);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index), authorIndex);
            }

            authorIndex.Authors = _context.Authors.ToList();
            return View(nameof(Index), authorIndex);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("ID, FullName")] Author author)
        {
            if (id != author.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(author);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                AuthorIndex authorIndex = new AuthorIndex { Authors = _context.Authors.ToList() };
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "Index", authorIndex) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", author) });
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = _context.Authors
                .FirstOrDefault(m => m.ID == id);
            if (author == null)
            {
                return NotFound();
            }
            _context.Authors.Remove(author);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.ID == id);
        }
    }
}
