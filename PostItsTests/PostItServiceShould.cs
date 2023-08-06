using Microsoft.EntityFrameworkCore;
using PostItDemo.Controllers;
using PostItDemo.Models;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace PostItsTests;

public class PostItServiceShould
{
    IPostItService _sut;
    PostItContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<PostItContext>().UseInMemoryDatabase(databaseName: "aspnet-PostIt").Options;
        _context = new PostItContext(options);

        //seed some dummy data
        _context.PostIts.Add(new PostIt()
        {
            Author = new Author() { Handle = "Dave", Passwd = "Secret" },
            Body = "Body",
            Title = "Title",
            Uploaded = new DateTime(1990, 01, 01)
        });
        _context.PostIts.Add(new PostIt()
        {
            Author = new Author() { Handle = "Jim", Passwd = "Secret" },
            Body = "Body 2",
            Title = "Title 2",
            Uploaded = new DateTime(1990, 01, 02)
        });
        _context.SaveChanges();

        _sut = new PostItService(_context);
    }

    [TearDown]
    public void TearDown()
    {
        var postIts = _context.PostIts.ToList();
        var authors = _context.Authors.ToList();
        var authorLikes = _context.AuthorLikes.ToList();
        _context.AuthorLikes.RemoveRange(authorLikes);
        _context.Authors.RemoveRange(authors);
        _context.PostIts.RemoveRange(postIts);
    }

    [Test]
    public void WhenAdd_WithValidData_ReturnTrue()
    {
        //arrange
        var newData = NewPost();

        //act
        var result = _sut.Add(newData);

        //assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void WhenAdd_WithInValidData_ReturnFalse()
    {
        //arrange
        var badData = new PostIt() { };

        //act
        var result = _sut.Add(badData);

        //assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void WhenAdd_WithValidData_IncreasePostItsBy1()
    {
        //arrange
        var newData = NewPost();
        var prior = _context.PostIts.ToList().Count;

        //act
        _sut.Add(newData);
        var post = _context.PostIts.ToList().Count;

        //assert
        Assert.That(post, Is.EqualTo(prior + 1));
    }

    [Test]
    public void WhenGet_WithValidId_ReturnsPostIt()
    {
        //arrange
        int id = _context.PostIts.ToList().First().PostItId;

        //act
        var postIt = _sut.Get(id);

        //assert
        Assert.That(postIt, Is.Not.Null);
        Assert.That(postIt.PostItId, Is.EqualTo(id));
    }

    [Test]
    public void WhenGet_WithInValidId_ReturnsNull()
    {
        //arrange
        int id = -1;

        //act
        var postIt = _sut.Get(id);

        //assert
        Assert.That(postIt, Is.Null);
    }

    [Test]
    public void WhenGetAll_ReturnsAllPostIts()
    {
        //arrange
        var allPostItsApriori = _context.PostIts.ToList();

        //act
        var allPostItsToAssert = _sut.GetAll();

        //assert
        Assert.That(allPostItsToAssert, Is.Not.Null);
        Assert.That(allPostItsApriori.Count, Is.EqualTo(allPostItsToAssert.Count));
    }

    [Test]
    public void WhenGetAll_ReturnsAllPostIts_WithAuthorAndAuthorLikes()
    {
        //arrange a new post in the context with an identifiable author and authorlike
        var postIt = NewPost();
        var author = _context.Authors.ToList().First();
        postIt.Author = author;
        postIt.AuthorLikes = new List<AuthorLike>()
        {
            new AuthorLike()
            {
                Author = author,
                PostIt = postIt
            }
        };

        //grab the entity for identification
        postIt = _context.PostIts.Add(postIt).Entity;
        _context.SaveChanges();

        //act & null check
        var allPostIts = _sut.GetAll();
        Assert.That(allPostIts, Is.Not.Null);
        var postUnderTest = allPostIts.Find(p => p.PostItId == postIt.PostItId);

        //assert
        Assert.That(postUnderTest, Is.Not.Null);
        Assert.That(postUnderTest.Author, Is.Not.Null);
        Assert.That(postUnderTest.Author.Id, Is.EqualTo(author.Id));

        Assert.That(postUnderTest.AuthorLikes, Is.Not.Null);
        Assert.That(postUnderTest.AuthorLikes.Count, Is.EqualTo(1));
        Assert.That(postUnderTest.AuthorLikes.First().Id, Is.EqualTo(postIt.AuthorLikes!.First().Id));
    }

    [Test]
    public void WhenPostItExists_WithValidId_ReturnsTrue()
    {
        //arrange
        int id = _context.PostIts.ToList().First().PostItId;

        //act
        var postIt = _sut.PostItExists(id);

        //assert
        Assert.That(postIt, Is.True);
    }

    [Test]
    public void WhenPostItExists_WithInValidId_ReturnsFalse()
    {
        //arrange
        int id = -1;

        //act
        var postIt = _sut.PostItExists(id);

        //assert
        Assert.That(postIt, Is.False);
    }

    [Test]
    public void WhenGetAuthor_WithValidHandle_ReturnsAuthor()
    {
        //arrange
        string handle = "Dave";

        //act
        var author = _sut.GetAuthor(handle);

        //assert
        Assert.That(author, Is.Not.Null);
        Assert.That(author.Handle, Is.EqualTo(handle));
    }

    [Test]
    public void WhenGetAuthor_WithInValidHandle_ReturnsNull()
    {
        //arrange
        string handle = "Wrong";

        //act
        var author = _sut.GetAuthor(handle);

        //assert
        Assert.That(author, Is.Null);
    }

    [Test]
    public void WhenGetUserAuthor_WithInValidClaimsPrincipal_ReturnsNull()
    {
        //arrange
        var invalidUser = new ClaimsPrincipal();

        //act
        var author = _sut.GetUserAuthor(invalidUser);

        //assert
        Assert.That(author, Is.Null);
    }

    [Test]
    public void WhenUpdate_WithValidModelState_ReturnsTrue()
    {
        //arrange
        var existingPost = _context.PostIts.First();
        existingPost.Title = "New Title";

        //act
        var result = _sut.Update(existingPost);

        //assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void WhenUpdate_WithValidModelState_UpdatesPostItCorrectly()
    {
        //arrange
        var existingPost = _context.PostIts.First();
        existingPost.Title = "New Title";

        //act
        _sut.Update(existingPost);
        var updatedPost = _context.PostIts.First();

        //assert
        Assert.That(updatedPost.Title, Is.EqualTo("New Title"));
    }

    [Test]
    public void WhenUpdate_WithInValidModelState_ReturnsFalse()
    {
        //arrange
        var badPost = new PostIt();

        //act
        var result = _sut.Update(badPost);

        //assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void WhenRemove_WithValidId_ReturnsTrue()
    {
        //arrange
        int id = _context.PostIts.ToList().First().PostItId;

        //act
        var result = _sut.Remove(id);

        //assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void WhenRemove_WithInValidId_ReturnsFalse()
    {
        //arrange
        int id = -1;

        //act
        var result = _sut.Remove(id);

        //assert
        Assert.That(result, Is.False);
    }

    private PostIt NewPost()
    {
        return new PostIt()
        {
            Author = NewAuthor(),
            Body = "Body 3",
            Title = "Title 3",
            Uploaded = new DateTime(1990, 01, 03)
        };
    }

    private Author NewAuthor()
    {
        return new Author() { Handle = "Tom", Passwd = "Secret" };
    }
}
