namespace PostItDemo.Models
{
    public class HomePageDTO : Author
    {
        public bool Error { get; set; } = false;
        public bool NewlyRegistered { get; set; } = false;
        public string? ErrorMessage { get; set; }

        public HomePageDTO() { }

        public HomePageDTO(Author author)
        {
            Handle = author.Handle;
            Passwd = author.Passwd;
            Id = author.Id;
        }

        public Author ToAuthor()
        {
            return new Author() {
                Id = this.Id,
                Handle = this.Handle,
                Passwd = this.Passwd
            };
        }

        public override string ToString()
        {
            return $"{Id} {Handle} {Passwd} Error:{Error} ErrorMessage:{ErrorMessage} NewlyRegistered:{NewlyRegistered}";
        }
    }
}
