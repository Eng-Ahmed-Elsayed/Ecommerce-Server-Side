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
        public string Provider { get; set; }
        //Card Holder Name
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

        [Required]
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
