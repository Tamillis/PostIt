using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostItDemo.Models;
using System.Diagnostics;

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
            var postIts = await _context.PostIts.Include(p => p.Author).ToListAsync();
            List<PostDTO> posts = new();

            foreach(var postIt in postIts)
            {
                posts.Add(new PostDTO(postIt));
            }

            return View(posts);
        }

        // POST: PostIts
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("Title,Body,Handle")] PostDTO postDTO)
        {
            postDTO.Author = _context.Authors.Where(a => a.Handle == postDTO.Handle).FirstOrDefault();

            postDTO.Uploaded = DateTime.Now.Date;

            if (ModelState.IsValid)
            {
                _context.Add(postDTO.ToPostIt());
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Edit(int id, [Bind("PostItId,Title,Body")] PostIt postIt)
        {
            if (id != postIt.PostItId)
            {
                return NotFound();
            }

            postIt.Uploaded = DateTime.Now.Date;

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
            if (id == null || _context.PostIts == null) return NotFound();

            var postIt = await _context.PostIts.FirstOrDefaultAsync(m => m.PostItId == id);
            if (postIt == null) return NotFound();

            return View(postIt);
        }

        // POST: PostIts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Debug.WriteLine("DeleteConfirmed " + id);
            if (_context.PostIts == null)
            {
                return Problem("Entity set 'PostItContext.PostIts'  is null.");
            }

            var postIt = await _context.PostIts.FindAsync(id);
            if (postIt != null)
            {
                _context.PostIts.Remove(postIt);
                await _context.SaveChangesAsync();
            }
            else return Problem($"Entity at Id {id} not found");

            return RedirectToAction(nameof(Index));
        }

        private bool PostItExists(int id)
        {
            return (_context.PostIts?.Any(e => e.PostItId == id)).GetValueOrDefault();
        }
    }
}
