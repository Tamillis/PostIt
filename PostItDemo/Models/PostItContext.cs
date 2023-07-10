using Microsoft.EntityFrameworkCore;

namespace PostItDemo.Models
{
    public partial class PostItContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<PostIt> PostIts { get; set; }

        public PostItContext(DbContextOptions<PostItContext> options) : base(options) { }
    }
}
