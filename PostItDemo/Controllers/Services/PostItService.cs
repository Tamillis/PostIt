using Microsoft.EntityFrameworkCore;
using PostItDemo.Models;
using System.Security.Claims;

namespace PostItDemo.Controllers
{
    public class PostItService : IPostItService
    {

        private readonly PostItContext _context;

        public PostItService(PostItContext context)
        {
            _context = context;
        }

        public bool Add(PostIt postIt)
        {
            try
            {
                _context.PostIts.Add(postIt);
                return SaveChanges();
            }
            catch
            {
                return false;
            }
        }

        public PostIt? Get(int id)
        {
            return _context.PostIts
                .Include(p => p.Author)
                .Include(p => p.AuthorLikes)
                .Where(p => p.PostItId == id)
                .FirstOrDefault();
        }

        public List<PostIt>? GetAll()
        {
            return _context.PostIts
                .Include(p => p.Author)
                .Include(p => p.AuthorLikes)
                .ToList();
        }

        public Author? GetAuthor(string handle)
        {
            return _context.Authors.Where(a => a.Handle == handle).FirstOrDefault();
        }

        public Author? GetUserAuthor(ClaimsPrincipal user)
        {
            if (!Utils.UserHasHandle(user)) return null;
            return _context.Authors.Where(a => a.Handle == Utils.GetUserHandle(user)).FirstOrDefault();
        }

        public bool PostItExists(int id)
        {
            return (_context.PostIts?.Any(e => e.PostItId == id)).GetValueOrDefault();
        }

        public bool Remove(int id)
        {
            try
            {
                _context.Remove(_context.PostIts.Find(id)!);
                return SaveChanges();
            }
            catch
            {
                return false;
            }
        }

        public bool SaveChanges()
        {
            try
            {
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(PostIt postIt)
        {
            try
            {
                _context.Update(postIt);
                SaveChanges();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostItExists(postIt.PostItId))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
