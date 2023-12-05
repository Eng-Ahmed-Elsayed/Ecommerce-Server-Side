using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Models.Models
{
    public class Discount
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(40,
            MinimumLength = 5,
            ErrorMessage = "Name length must be between 5 and 40.")]
        public string Name { get; set; }
        [Required]
        [Range(0.01, 0.99)]
        [Precision(3, 2)]
        public decimal DiscountPercent { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public List<Product> Products { get; } = new List<Product>();

        [Required]
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
