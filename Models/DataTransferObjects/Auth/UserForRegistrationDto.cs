using System.ComponentModel.DataAnnotations;

namespace Models.DataTransferObjects.Auth
{
    public class UserForRegistrationDto
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(20,
            MinimumLength = 3,
            ErrorMessage = "Username length must be between 3 and 40.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(20,
            MinimumLength = 7,
            ErrorMessage = "Password length must be between 3 and 40.")]
        public string? Password { get; set; }
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match")]
        public string? ConfirmPassword { get; set; }
        public DateTime? Birthday { get; set; }
        public string? ClientURI { get; set; }
    }
}
