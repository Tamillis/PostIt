using System.ComponentModel;

namespace PostItDemo.Models
{
    public class Author
    {
        public int Id { get; set; }

        public string Handle { get; set; }

        [DisplayName("Password")]
        public string Passwd { get; set; }

        public ICollection<PostIt>? PostIts { get; set; }

        public ICollection<AuthorLike>? AuthorLikes { get; set; }

    }
}
