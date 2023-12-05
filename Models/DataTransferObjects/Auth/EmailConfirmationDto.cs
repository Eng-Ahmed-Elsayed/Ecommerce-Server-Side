using System.ComponentModel.DataAnnotations;

namespace Models.DataTransferObjects.Auth
{
    public class EmailConfirmationDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Token { get; set; }
    }
}
