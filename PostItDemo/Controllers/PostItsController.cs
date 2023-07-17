using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostItDemo.Models;
using System.Diagnostics;

namespace PostItDemo.Controllers
{
    public class PostItsController : Controller
    {
        private readonly PostItContext _context; 
        private readonly ILogger<PostItsController> _logger;

        public PostItsController(ILogger<PostItsController> logger, PostItContext context)
        {
            _context = context;
            _logger = logger;
        }

        // GET: PostIts/{id} (Id is optional, in which case all posts are fetched)
        public async Task<IActionResult> Index(int? id)
        {
            var postIts = await _context.PostIts.Include(p => p.Author).Include(p=>p.AuthorLikes).ToListAsync();

            if(id != null)
            {
                postIts = postIts.Where(p => p.PostItId == id).ToList();
            }

            if (postIts == null)
            {
                return NotFound();
            }

            List<PostDTO> posts = new();

            foreach(var postIt in postIts)
            {
                var postDTO = new PostDTO(postIt);
                if (Utils.UserHasHandle(HttpContext.User)) {
                    postDTO.UserAuthor = await _context.Authors
                        .Where(a => a.Handle == Utils.GetUserHandle(HttpContext.User))
                        .FirstOrDefaultAsync();
                    postDTO.AuthorLikes = await _context.AuthorLikes.Where(al => al.PostIt.PostItId == postDTO.PostItId).ToListAsync();
                }
                posts.Add(postDTO);
            }

            //order by algorithm implemented in PostDTO comparable implementation
            posts.Sort();

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

        // POST: PostIts/Like/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Like(int id)
        {
            if (_context.PostIts == null)
            {

                return RedirectToAction(nameof(Index));
            }

            var postIt = await _context.PostIts.FindAsync(id);
            if (postIt == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var userAuthor = await GetUserAuthor();
            if(userAuthor is null) return RedirectToAction(nameof(Index));

            var newAuthorLike = new AuthorLike()
            {
                Author = userAuthor,
                PostIt = postIt
            };

            postIt.AuthorLikes ??= new List<AuthorLike>();

            postIt.AuthorLikes.Add(newAuthorLike);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: PostIts/Unlike/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unlike(int id)
        {
            Debug.WriteLine("Unlike " + id);

            var postIt = await _context.PostIts
                .Include(p=>p.AuthorLikes)
                .ThenInclude(al => al.Author)
                .Where(p=>p.PostItId==id)
                .FirstOrDefaultAsync();

            var userAuthor = await GetUserAuthor();

            if (postIt != null && postIt.AuthorLikes != null && userAuthor != null)
            {
                var likesToRemove = postIt.AuthorLikes.Where(al => al.Author.Id == userAuthor.Id);
                
                foreach(var like in likesToRemove)
                {
                    postIt.AuthorLikes.Remove(like);
                }

                await _context.SaveChangesAsync();
            }
            else return Problem($"Entity at Id {id} not found");

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

        private async Task<Author?> GetUserAuthor()
        {
            if (!Utils.UserHasHandle(HttpContext.User)) return null;
            return await _context.Authors.Where(a => a.Handle == Utils.GetUserHandle(HttpContext.User)).FirstOrDefaultAsync();
        }
    }
}
