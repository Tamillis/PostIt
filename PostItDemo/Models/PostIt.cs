using Microsoft.AspNetCore.Mvc;

namespace PostItDemo.Models
{
    [BindProperties]
    public class PostIt
    {
        public int PostItId { get; set; }
        public int? MotherPostIt { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        public Author? Author { get; set; }

        public DateTime Uploaded { get; set; }

        public PostIt() { }

        public PostIt(PostIt p)
        {
            PostItId = p.PostItId;
            MotherPostIt = p.MotherPostIt;
            Title = p.Title;
            Body = p.Body;
            Author = p.Author;
            Uploaded = p.Uploaded;
        }
    }
}
