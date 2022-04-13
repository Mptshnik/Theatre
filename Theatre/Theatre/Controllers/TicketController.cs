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
    public class TicketController : Controller
    {
        private readonly DatabaseContext _context;

        public TicketController(DatabaseContext context)
        {
            _context = context;
        }


        public IActionResult Index(int role)
        {
            ViewData["PerformanceID"] = new SelectList(_context.Performances, "ID", "Name");
            ViewData["CurrentRole"] = role;
            var databaseContext = _context.Tickets.Include(t => t.Performance);
            TicketIndex ticketIndex = new TicketIndex
            {
                Tickets = databaseContext.ToList(),
                CurrentRole = role           
            };
            return View(ticketIndex);
        }

     
        [HttpPost]
        public IActionResult Create(TicketIndex ticketIndex)
        {
            bool SeatExists = _context.Tickets.Any
                (x => x.SeatNumber == ticketIndex.Ticket.SeatNumber && x.ID != ticketIndex.Ticket.ID);

            if (SeatExists)
            {
                ModelState.AddModelError("SeatNumber", "");
                ViewData["SeatExists"] = "Номер места занят";
            }

            ViewData["CurrentRole"] = ticketIndex.CurrentRole;
            if (ModelState.IsValid)
            {
                _context.Tickets.Add(ticketIndex.Ticket);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PerformanceID"] = new SelectList(_context.Performances, "ID", "Name", ticketIndex.Ticket.PerformanceID);
            ticketIndex.Tickets = _context.Tickets.Include(t => t.Performance);
            return View(nameof(Index), ticketIndex);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["PerformanceID"] = new SelectList(_context.Performances, "ID", "Name", ticket.PerformanceID);
            return View(ticket);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Ticket ticket)
        {
            if (id != ticket.ID)
            {
                return NotFound();
            }

            bool SeatExists = _context.Tickets.Any
                (x => x.SeatNumber == ticket.SeatNumber && x.ID != ticket.ID);

            if (SeatExists)
            {
                ModelState.AddModelError("SeatNumber", "");
                ViewData["SeatExists"] = "Номер места занят";
            }


            ViewData["PerformanceID"] = new SelectList(_context.Performances, "ID", "Name", ticket.PerformanceID);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Tickets.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TicketIndex ticketIndex = new TicketIndex { Tickets = _context.Tickets.Include(t => t.Performance) };
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "Index", ticketIndex) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", ticket) });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Performance)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (ticket == null)
            {
                return NotFound();
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.ID == id);
        }
    }
}
