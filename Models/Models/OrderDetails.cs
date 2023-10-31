using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Models.Models
{
    public class OrderDetails
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public ICollection<OrderItem>? OrderItems { get; } = new List<OrderItem>();

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Precision(18, 2)]
        public decimal? Total
        {
            get
            {
                decimal? total = 0;
                foreach (var item in OrderItems)
                {
                    total += item.Price;
                }
                return total;
            }
        }

        public Guid ShippingOptionId { get; set; }
        public ShippingOption? ShippingOption { get; }
        public Guid UserAddressId { get; set; }
        public UserAddress? UserAddress { get; }
        public Guid UserPaymentId { get; set; }
        public UserPayment? UserPayment { get; }


        [Required]
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
