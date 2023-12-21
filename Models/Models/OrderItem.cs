using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Models.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid OrderDetailsId { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        [Required]
        [Range(1, 999999)]
        public int Quantity { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Precision(18, 2)]
        public decimal? Price
        {
            get
            {
                if (DiscountPercent != null)
                {
                    return Product.Price * Quantity * (1 - DiscountPercent);
                }
                return Product.Price * Quantity;
            }
        }

        [StringLength(6,
            MinimumLength = 6,
            ErrorMessage = "Discount code length must be 6.")]
        public string? DiscountCode { get; set; }
        [Range(0.01, 0.99)]
        [Precision(3, 2)]
        public decimal? DiscountPercent { get; set; }

        [Required]
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
