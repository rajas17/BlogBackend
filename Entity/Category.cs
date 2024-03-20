using System.ComponentModel.DataAnnotations;

namespace BlogBackend.Entity
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        public string CategoryName { get; set; }

        public string Status { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
