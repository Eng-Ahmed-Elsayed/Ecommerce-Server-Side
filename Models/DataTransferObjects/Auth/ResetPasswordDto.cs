using System.ComponentModel.DataAnnotations;

namespace Models.DataTransferObjects.Auth
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "New password is required.")]
        [StringLength(20,
            MinimumLength = 7,
            ErrorMessage = "Password length must be between 3 and 40.")]
        public string? Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "The password and confirm password do not match.")]
        public string? ConfirmPassword { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public string? Token { get; set; }
    }
}
