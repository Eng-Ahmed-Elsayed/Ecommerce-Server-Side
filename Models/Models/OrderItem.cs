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
        public Product Product { get; }
        [Required]
        [Range(1, 999999)]
        public int Quantity { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Precision(18, 2)]
        public decimal? Price
        {
            get
            {
                return Product.Price * Quantity;
            }
        }

        [Required]
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
