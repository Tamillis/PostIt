using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace PostItDemo.Models
{
    [BindProperties]
    public class AuthorLike
    {
        public int Id { get; set; }
        public PostIt PostIt { get; set; }
        public Author Author { get; set; }

        
    }
}
