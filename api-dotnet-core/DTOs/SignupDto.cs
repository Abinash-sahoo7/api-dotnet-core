using System.ComponentModel.DataAnnotations;

namespace api_dotnet_core.DTOs
{
    public class SignupDto
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }


        [Required]
        [EmailAddress]
        public string Email { get; set; }


        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
