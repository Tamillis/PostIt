namespace PostItDemo.Models
{
    public class PostIt
    {
        public int PostItId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        public Author Author { get; set; }

        public DateTime Uploaded { get; set; }
    }
}
