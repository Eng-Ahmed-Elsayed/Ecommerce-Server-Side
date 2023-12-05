using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class UserPayment
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public User? User { get; }
        //[Required]
        //public string PaymentType { get; set; }
        [Required]
        [StringLength(40,
            MinimumLength = 3,
            ErrorMessage = "Provider length must be between 3 and 40.")]
        public string Provider { get; set; }
        //Card Holder Name
        [Required]
        [StringLength(40,
            MinimumLength = 3,
            ErrorMessage = "Name length must be between 3 and 40.")]
        public string Name { get; set; }

        // Matches 9999-9999-9999-9999
        [RegularExpression("^[0-9]{4}-[0-9]{4}-[0-9]{4}-[0-9]{4}$")]
        [Required]
        [StringLength(19,
            MinimumLength = 19,
            ErrorMessage = "Account number length must be 16.")]
        public string AccountNo { get; set; }
        //regular expression that matches the month/year pattern
        //This will match strings like 01/21, 12/2023, 02/02, 11/1111
        //, etc. But it will not match strings like 13/21, 01/3, 1/21, 01/202, etc.
        [RegularExpression("^(0[1-9]|1[0-2])\\/([0-9]{2}|[0-9]{4})$")]
        [Required]
        public string Expiry { get; set; }
        [Required]
        [StringLength(3,
            MinimumLength = 3,
            ErrorMessage = "CVV number length must be 3.")]
        public string Cvv { get; set; }

        [Required]
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
