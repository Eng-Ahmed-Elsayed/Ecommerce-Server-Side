using System.ComponentModel.DataAnnotations;

namespace Models.DataTransferObjects.Auth
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        public string? ClientURI { get; set; }
    }
}
