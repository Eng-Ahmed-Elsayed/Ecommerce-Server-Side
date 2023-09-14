using System.ComponentModel.DataAnnotations;

namespace Models.DataTransferObjects.Auth
{
    public class ExternalAuthDto
    {
        [Required]
        public string? Provider { get; set; }
        [Required]
        public string? IdToken { get; set; }
    }
}
