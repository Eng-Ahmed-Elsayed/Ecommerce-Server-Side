using System.ComponentModel.DataAnnotations;

namespace Models.DataTransferObjects
{
    public class UserForAuthenticationDto
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(20,
            MinimumLength = 3,
            ErrorMessage = "Username length must be between 3 and 40.")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(20,
            MinimumLength = 7,
            ErrorMessage = "Password length must be between 3 and 40.")]
        public string? Password { get; set; }
        public string? ClientURI { get; set; }
    }
}
