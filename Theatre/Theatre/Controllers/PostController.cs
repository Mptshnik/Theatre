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
    public class PostController : Controller
    {
        private readonly DatabaseContext _context;

        public PostController(DatabaseContext context)
        {
            _context = context;
        }

 
        public IActionResult Index(int role)
        {
            ViewData["CurrentRole"] = role;
            PostIndex postIndex = new PostIndex()
            {
                Posts = _context.Posts.ToList(),
                CurrentRole = role
            };

            return View(postIndex);
        }


        [HttpPost]
        public async Task<IActionResult> Create(PostIndex postIndex)
        {
            ViewData["CurrentRole"] = postIndex.CurrentRole;
            if (ModelState.IsValid)
            {
                _context.Add(postIndex.Post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            postIndex.Posts = _context.Posts.ToList();
            return View(nameof(Index), postIndex);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name")] Post post)
        {
            if (id != post.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                PostIndex postIndex = new PostIndex { Posts = _context.Posts.ToList() };
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "Index", postIndex) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", post) });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FirstOrDefaultAsync(m => m.ID == id);
            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.ID == id);
        }
    }
}
