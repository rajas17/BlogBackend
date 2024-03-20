using System.ComponentModel.DataAnnotations;

namespace BlogBackend.DTO
{
    public class adminDTO
    {
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
