using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Models.Models
{
    public class ShoppingCart
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public List<CartItem>? CartItems { get; } = new();

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Precision(18, 2)]
        public decimal? Total
        {
            get
            {
                decimal? total = 0;
                foreach (var item in CartItems)
                {
                    total += item.Price;
                }
                return total;
            }
        }

        [Required]
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }

    }
}
