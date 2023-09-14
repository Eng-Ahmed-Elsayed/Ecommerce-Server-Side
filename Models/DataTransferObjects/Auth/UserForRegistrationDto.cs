using System.ComponentModel.DataAnnotations;

namespace Models.DataTransferObjects.Auth
{
    public class UserForRegistrationDto
    {
        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set; }
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match")]
        public string? ConfirmPassword { get; set; }
        public string? Birthday { get; set; }
        public string? ClientURI { get; set; }
    }
}
