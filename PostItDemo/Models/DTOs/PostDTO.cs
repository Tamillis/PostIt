using PostItDemo.Controllers;

namespace PostItDemo.Models
{
    public class PostDTO : PostIt, IEquatable<PostDTO>, IComparable<PostDTO>
    {
        public string Handle { get; set; } = "Anon";
        public Author? UserAuthor { get; set; } = null;

        public ICollection<PostDTO> ChildPosts { get; set; } = new List<PostDTO>();

        public int Replies { get
            {
                return ChildPosts.Count;
            } }

        public PostDTO(PostIt p, Author? userAuthor = null, ICollection<AuthorLike>? authorLikes = null) : base(p) {
            if (p.Author is not null) Handle = p.Author.Handle;
            if (userAuthor is not null) UserAuthor = userAuthor;
            if (authorLikes is not null) AuthorLikes = authorLikes;
        }

        public PostDTO() { }

        public bool PostIsLikedByUser()
        {
            if (AuthorLikes is null || UserAuthor is null) return false;
            return AuthorLikes.Where(al => al.Author.Id == UserAuthor.Id).ToList().Count > 0;
        }

        public PostIt ToPostIt()
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

        public bool Equals(PostDTO? other)
        {
            if (other is null) return false;

            return PostItId.Equals(other.PostItId);
        }

        public int CompareTo(PostDTO? other)
        {
            if (other is null) return 1;
            else if (Equals(other)) return 0;
            else if (Utils.GetPostValue(this) < Utils.GetPostValue(other)) return 1;
            else return -1;
        }
    }
}
