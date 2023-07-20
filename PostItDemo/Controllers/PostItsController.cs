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

        // GET: gets all posts
        public async Task<IActionResult> Index()
        {
            //get all posts
            List<PostIt> postIts = await _context.PostIts
                .Include(p => p.Author)
                .Include(p=>p.AuthorLikes)                
                .ToListAsync()!;

            if (postIts == null)
            {
                return NotFound();
            }

            //setup DTO with their extra data needed by the view; child posts, current user etc.
            List<PostDTO> posts = new();

            foreach(var postIt in postIts)
            {
                var authorLikes = 
                    await _context.AuthorLikes
                    .Where(al => al.PostIt.PostItId == postIt.PostItId)
                    .ToListAsync();

                var postDTO = new PostDTO(postIt, await GetUserAuthor(), authorLikes);

                postDTO.ChildPosts = GetChildPosts(postIt.PostItId, postIts);
                
                posts.Add(postDTO);
            }

            posts = posts.Where(p => p.MotherPostIt == 0).ToList();

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
                _context.PostIts.Add(postDTO.ToPostIt());
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: PostIts/{id} View one particular posts, and its direct replies
        [HttpGet, ActionName("View")]
        public async Task<IActionResult> ViewDetails(int id)
        {
            var post = await _context.PostIts
                .Include(p => p.Author)
                .Include(p => p.AuthorLikes)
                .Where(p => p.PostItId == id)
                .FirstOrDefaultAsync();
            var postIts = await _context.PostIts
                .Include(p => p.Author)
                .Include(p => p.AuthorLikes)
                .ToListAsync();

            if (post == null)
            {
                return NotFound();
            }

            //setup DTO with their extra data needed by the view; child posts, current user etc.

            var authorLikes =
                await _context.AuthorLikes
                .Where(al => al.PostIt.PostItId == post.PostItId)
                .ToListAsync();

            var postDTO = new PostDTO(post, await GetUserAuthor(), authorLikes);

            postDTO.ChildPosts = GetChildPosts(post.PostItId, postIts);

            return View(postDTO);
        }

        // POST: View
        [HttpPost, ActionName("View")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ViewDetails([Bind("Title,Body,Handle,MotherPostIt")] PostDTO postDTO)
        {
            postDTO.Author = await GetUserAuthor();

            postDTO.Uploaded = DateTime.Now.Date;

            _logger.LogInformation($"{postDTO.MotherPostIt}");

            if (ModelState.IsValid)
            {
                _context.PostIts.Add(postDTO.ToPostIt());
                await _context.SaveChangesAsync();
            }
            else
            {
                Problem($"ModelState is invalid for {postDTO}");
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

        private List<PostDTO> GetChildPosts(int postItId, List<PostIt> allPosts)
        {
            var childPosts = allPosts.Where(p => p.MotherPostIt == postItId).ToList();
            var childPostDTOs = new List<PostDTO>();

            foreach (var post in childPosts)
            {
                var childPostDTO = new PostDTO(post);

                childPostDTO.ChildPosts = GetChildPosts(post.PostItId, allPosts);
                childPostDTOs.Add(childPostDTO);
            }

            return childPostDTOs;
        }
    }
}
