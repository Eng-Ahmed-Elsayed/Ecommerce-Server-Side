using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class PaymentDetails
    {
        public Guid Id { get; set; }
        public Guid OrderDetailsId { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public string Provider { get; set; }
        [Required]
        public string Status { get; set; }


        [Required]
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
