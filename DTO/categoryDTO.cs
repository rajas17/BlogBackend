using System.ComponentModel.DataAnnotations;

namespace BlogBackend.DTO
{
    public class categoryDTO
    {
        [Required]
        public string CategoryName { get; set; }

        public string Status { get; set; }
    }
}
