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
        [StringLength(40,
            MinimumLength = 3,
            ErrorMessage = "Provider length must be between 3 and 40.")]

        public string Provider { get; set; }
        [Required]
        [StringLength(40,
            MinimumLength = 3,
            ErrorMessage = "Name length must be between 3 and 40.")]

        public string Name { get; set; }

        [Required]
        [StringLength(19,
            MinimumLength = 19,
            ErrorMessage = "Account number length must be 16.")]

        public string AccountNo { get; set; }
        [RegularExpression("^(0[1-9]|1[0-2])\\/([0-9]{2}|[0-9]{4})$")]
        [Required]
        public string Expiry { get; set; }
        [Required]
        [StringLength(3,
            MinimumLength = 3,
            ErrorMessage = "CVV number length must be 3.")]
        public string Cvv { get; set; }

        public bool? Remember { get; set; }
    }
}
