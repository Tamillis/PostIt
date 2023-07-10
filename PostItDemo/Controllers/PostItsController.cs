using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PostItDemo.Models;

namespace PostItDemo.Controllers
{
    public class PostItsController : Controller
    {
        private readonly PostItContext _context;

        public PostItsController(PostItContext context)
        {
            _context = context;
        }

        // GET: PostIts
        public async Task<IActionResult> Index()
        {
              return _context.PostIts != null ? 
                          View(await _context.PostIts.ToListAsync()) :
                          Problem("Entity set 'PostItContext.PostIts'  is null.");
        }

        // GET: PostIts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PostIts == null)
            {
                return NotFound();
            }

            var postIt = await _context.PostIts
                .FirstOrDefaultAsync(m => m.PostItId == id);
            if (postIt == null)
            {
                return NotFound();
            }

            return View(postIt);
        }

        // GET: PostIts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PostIts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostItId,Title,Body,Uploaded")] PostIt postIt)
        {
            if (ModelState.IsValid)
            {
                _context.Add(postIt);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(postIt);
        }

        // GET: PostIts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PostIts == null)
            {
                return NotFound();
            }

            var postIt = await _context.PostIts.FindAsync(id);
            if (postIt == null)
            {
                return NotFound();
            }
            return View(postIt);
        }

        // POST: PostIts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostItId,Title,Body,Uploaded")] PostIt postIt)
        {
            if (id != postIt.PostItId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(postIt);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostItExists(postIt.PostItId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(postIt);
        }

        // GET: PostIts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PostIts == null)
            {
                return NotFound();
            }

            var postIt = await _context.PostIts
                .FirstOrDefaultAsync(m => m.PostItId == id);
            if (postIt == null)
            {
                return NotFound();
            }

            return View(postIt);
        }

        // POST: PostIts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PostIts == null)
            {
                return Problem("Entity set 'PostItContext.PostIts'  is null.");
            }
            var postIt = await _context.PostIts.FindAsync(id);
            if (postIt != null)
            {
                _context.PostIts.Remove(postIt);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostItExists(int id)
        {
          return (_context.PostIts?.Any(e => e.PostItId == id)).GetValueOrDefault();
        }
    }
}
