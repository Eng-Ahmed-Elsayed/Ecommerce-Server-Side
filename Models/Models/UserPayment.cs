using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class UserPayment
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
        [Required]
        public string PaymentType { get; set; }
        [Required]
        public string Provider { get; set; }
        [Required]
        public int AccountNo { get; set; }
        [Required]
        public DateTime Expiry { get; set; }

        [Required]
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
