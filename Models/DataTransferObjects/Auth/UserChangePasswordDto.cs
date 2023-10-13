using System.ComponentModel.DataAnnotations;

namespace Models.DataTransferObjects.Auth
{
    public class UserChangePasswordDto
    {
        [Required(ErrorMessage = "Current password is required.")]
        public string? CurrentPassword { get; set; }
        [Required]
        public string? Password { get; set; }
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match")]
        [Required]
        public string? ConfirmPassword { get; set; }
    }
}
