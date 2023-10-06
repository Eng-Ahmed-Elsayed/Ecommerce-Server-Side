using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Models.Models
{
    public class OrderDetails
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        [Required]
        [Precision(18, 2)]
        public decimal Total { get; set; }
        public Guid PaymentId { get; set; }
        public ICollection<OrderItem> OrderItems { get; } = new List<OrderItem>();


        [Required]
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
