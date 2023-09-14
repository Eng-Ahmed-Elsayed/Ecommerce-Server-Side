using System.ComponentModel.DataAnnotations;

namespace Models.DataTransferObjects.Auth
{
    public class LogoutDto
    {
        [Required]
        public string? Email { get; set; }
    }
}
