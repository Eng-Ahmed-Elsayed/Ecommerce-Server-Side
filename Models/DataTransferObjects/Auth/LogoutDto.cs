using System.ComponentModel.DataAnnotations;

namespace Models.DataTransferObjects.Auth
{
    public class LogoutDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
