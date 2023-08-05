using PostItDemo.Models;
using System.Security.Claims;

namespace PostItDemo.Controllers;

public interface IPostItService
{
    public List<PostIt>? GetAll();

    public PostIt? Get(int id);

    public bool PostItExists(int id);

    public Author? GetUserAuthor(ClaimsPrincipal user);

    public Author? GetAuthor(string handle);

    public bool Add(PostIt postIt);

    public bool Update(PostIt postIt);

    public bool Remove(int id);

    public bool SaveChanges();
}
