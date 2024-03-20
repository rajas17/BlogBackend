using BlogBackend.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogBackend.DTO
{
    public class addPostDTO
    {
        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string Permalink { get; set; }

        public string Content { get; set; }

        public string AuthorName { get; set; }

        public string AuthorRole { get; set; }

        public string ImgPath { get; set; }

        public int views { get; set;}

        public bool IsFeatured { get; set; }

        public DateTime CreatedAt { get; set; }

        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }


    }
}
