using System.ComponentModel.DataAnnotations;

namespace Models.DataTransferObjects
{
    public class SendEmailConfirmationDto
    {
        [Required(ErrorMessage = "Email is required.")]
        public string? Email { get; set; }
        public string? ClientURI { get; set; }

    }
}
