using System.ComponentModel.DataAnnotations;

namespace Models.DataTransferObjects
{
    public class LogoutDto
    {
        [Required]
        public string? Email { get; set; }
    }
}
