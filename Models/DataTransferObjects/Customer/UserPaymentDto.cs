using System.ComponentModel.DataAnnotations;
using Models.DataTransferObjects.Auth;

namespace Models.DataTransferObjects.Shared
{
    public class UserPaymentDto
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public UserDto? User { get; set; }

        [Required]
        public string Provider { get; set; }
        [Required]
        [MaxLength(80)]
        [MinLength(4)]
        public string Name { get; set; }

        [Required]
        [StringLength(16)]
        public string AccountNo { get; set; }
        [Required]
        public string Expiry { get; set; }
        [Required]
        [StringLength(3)]
        public string Cvv { get; set; }

        public bool? Remember { get; set; }
    }
}
