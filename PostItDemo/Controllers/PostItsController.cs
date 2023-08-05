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
        private readonly IPostItService _service;

        public PostItsController(ILogger<PostItsController> logger, PostItContext context, IPostItService service)
        {
            _context = context;
            _logger = logger;
            _service = service;
        }

        // GET: gets all posts
        public async Task<IActionResult> Index()
        {
            //get all posts
            List<PostIt>? postIts = _service.GetAll();

            if (postIts == null)
            {
                return NotFound();
            }

            //setup DTO with their extra data needed by the view; child posts, current user etc.
            List<PostDTO> posts = new();

            foreach (var postIt in postIts)
            {
                var postDTO = new PostDTO(postIt, _service.GetUserAuthor(HttpContext.User), postIt.AuthorLikes);

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
        public async Task<IActionResult> Index([Bind("Title,Body,Handle,MotherPostIt")] PostDTO postDTO)
        {
            postDTO.Author = _service.GetAuthor(postDTO.Handle);
            
            postDTO.Uploaded = DateTime.Now.Date;

            if (ModelState.IsValid)
            {
                _service.Add(postDTO.ToPostIt());
            }
            else
            {
                Problem($"ModelState for:\n {postDTO} \nis not valid");
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: PostIts/{id} View one particular posts, and its direct replies
        [HttpGet, ActionName("View")]
        public async Task<IActionResult> ViewDetails(int id)
        {
            var post = _service.Get(id);

            var postIts = _service.GetAll();

            if (post == null || postIts == null)
            {
                return NotFound();
            }

            var postDTO = new PostDTO(post, _service.GetUserAuthor(HttpContext.User), post.AuthorLikes);

            postDTO.ChildPosts = GetChildPosts(post.PostItId, postIts);

            return View(postDTO);
        }

        // POST: View
        [HttpPost, ActionName("View")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ViewDetails([Bind("Title,Body,Handle,MotherPostIt")] PostDTO postDTO)
        {
            postDTO.Author = _service.GetUserAuthor(HttpContext.User);

            postDTO.Uploaded = DateTime.Now.Date;

            if (ModelState.IsValid)
            {
                _service.Add(postDTO.ToPostIt());
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
            var postIt = _service.Get(id);

            if (postIt is null) return RedirectToAction(nameof(Index));

            var userAuthor = _service.GetUserAuthor(HttpContext.User);

            if (userAuthor is null) return RedirectToAction(nameof(Index));

            var newAuthorLike = new AuthorLike()
            {
                Author = userAuthor,
                PostIt = postIt
            };

            postIt.AuthorLikes ??= new List<AuthorLike>();

            postIt.AuthorLikes.Add(newAuthorLike);

            _service.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // POST: PostIts/Unlike/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unlike(int id)
        {
            var postIt = _service.Get(id);

            var userAuthor = _service.GetUserAuthor(HttpContext.User);

            if (postIt != null && postIt.AuthorLikes != null && userAuthor != null)
            {
                var likesToRemove = postIt.AuthorLikes.Where(al => al.Author.Id == userAuthor.Id);

                foreach (var like in likesToRemove)
                {
                    postIt.AuthorLikes.Remove(like);
                }

                _service.SaveChanges();
            }
            else return Problem($"Entity at Id {id} not found");

            return RedirectToAction(nameof(Index));
        }

        // GET: PostIts/Edit/5
        // Get Edit View
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return NotFound();
            
            var postIt = _service.Get((int)id);

            if (postIt is null) return NotFound();

            return View(postIt);
        }

        // POST: PostIts/Edit/5
        // Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostItId,Title,Body")] PostIt postIt)
        {
            if (id != postIt.PostItId || !_service.PostItExists(id))
            {
                return NotFound();
            }

            postIt.Uploaded = DateTime.Now.Date;

            if (!ModelState.IsValid) return Problem($"ModelState is invalid for {postIt}");

            if(_service.Update(postIt))
            {
                return View(postIt);
            }
            else return RedirectToAction(nameof(Index));
        }

        // POST: PostIts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogWarning("DeleteConfirmed " + id);

            var postIt = _service.Get(id);

            if (postIt != null)
            {
                if(!_service.Remove(id)) return Problem($"Entity at Id {id} not Removed successfully");
            }
            else return Problem($"Entity at Id {id} not found");

            return RedirectToAction(nameof(Index));
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
