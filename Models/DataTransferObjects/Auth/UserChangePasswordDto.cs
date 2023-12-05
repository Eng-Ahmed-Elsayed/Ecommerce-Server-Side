using System.ComponentModel.DataAnnotations;

namespace Models.DataTransferObjects.Auth
{
    public class UserChangePasswordDto
    {
        [Required(ErrorMessage = "Current password is required.")]
        [StringLength(20,
            MinimumLength = 7,
            ErrorMessage = "Password length must be between 3 and 40.")]
        public string? CurrentPassword { get; set; }
        [Required]
        [StringLength(20,
            MinimumLength = 7,
            ErrorMessage = "Password length must be between 3 and 40.")]
        public string? Password { get; set; }
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match")]
        [Required]
        public string? ConfirmPassword { get; set; }
    }
}
