using System.ComponentModel.DataAnnotations;

namespace Models.DataTransferObjects
{
    public class UserForAuthenticationDto
    {
        [Required(ErrorMessage = "UserName is required.")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set; }
        public string? ClientURI { get; set; }
    }
}
