namespace PostItDemo.Models
{
    public class PostDTO : PostIt
    {
        public string Handle { get; set; } = "Anon";

        internal object ToPostIt()
        {
            return new PostIt()
            {
                PostItId = this.PostItId,
                MotherPostIt = this.MotherPostIt,
                Title = this.Title,
                Body = this.Body,
                Author = this.Author,
                Uploaded = this.Uploaded
            };
        }
    }
}
