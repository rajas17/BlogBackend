using System.ComponentModel.DataAnnotations;

namespace BlogBackend.Entity
{
    public class Admin
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public bool IsMaster { get; set; }
    }
}
