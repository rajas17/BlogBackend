using System.ComponentModel.DataAnnotations.Schema;

namespace BlogBackend.Entity
{
    public class Post
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Permalink { get; set; }

        public string Subtitle { get; set; }

        public string content { get; set; }

        public string ImgPath { get; set; }

        public int Views { get; set; }

        public DateTime CreatedAt { get; set; }

        public string AuthorName { get; set; }

        public string AuthorRole { get; set; }

        public Boolean IsFeatured { get; set; }

        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public Category Category { get; set; }
    }
}
