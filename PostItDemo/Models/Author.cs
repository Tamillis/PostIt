namespace PostItDemo.Models
{
    public class Author
    {
        public int Id { get; set; }

        public string Handle { get; set; }

        public string Passwd { get; set; }

        public ICollection<PostIt>? PostIts { get; set; }

    }
}
