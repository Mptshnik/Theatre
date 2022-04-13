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
    public class MembershipController : Controller
    {
        private readonly DatabaseContext _context;

        public MembershipController(DatabaseContext context)
        {
            _context = context;
        }


        public IActionResult Index(int role)
        {
            ViewData["AuthorID"] = new SelectList(_context.Authors, "ID", "FullName");
            ViewData["CurrentRole"] = role;
            var databaseContext = _context.Memberships.Include(m => m.Author);
            MembershipIndex membershipIndex = new MembershipIndex { Memberships = databaseContext.ToList(), CurrentRole = role};
            return View(membershipIndex);
        }


        [HttpPost]
        public async Task<IActionResult> Create(MembershipIndex membershipIndex)
        {
            Membership membership = membershipIndex.Membership;
            ViewData["CurrentRole"] = membershipIndex.CurrentRole;
           
            if (ModelState.IsValid)
            {
                _context.Memberships.Add(membership);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorID"] = new SelectList(_context.Authors, "ID", "FullName", membership.AuthorID);
            membershipIndex.Memberships = _context.Memberships.ToList();
            return View(nameof(Index), membershipIndex);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membership = await _context.Memberships.FindAsync(id);

            ViewData["AuthorID"] = new SelectList(_context.Authors, "ID", "FullName", membership.AuthorID);
            if (membership == null)
            {
                return NotFound();
            }
      
            return View(membership);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("ID, Price, Genre, AuthorID")] Membership membership)
        {
            if (id != membership.ID)
            {
                return NotFound();
            }
            ViewData["AuthorID"] = new SelectList(_context.Authors, "ID", "FullName", membership.AuthorID);
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Memberships.Update(membership);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MembershipExists(membership.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                MembershipIndex membershipIndex = new MembershipIndex { Memberships = _context.Memberships.Include(e => e.Author) };
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "Index", membershipIndex) });
            }

            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", membership) });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membership = await _context.Memberships
                .Include(m => m.Author)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (membership == null)
            {
                return NotFound();
            }

            _context.Memberships.Remove(membership);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MembershipExists(int id)
        {
            return _context.Memberships.Any(e => e.ID == id);
        }
    }
}
