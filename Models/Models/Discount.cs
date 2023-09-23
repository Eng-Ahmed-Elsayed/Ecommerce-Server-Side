using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Models.Models
{
    public class Discount
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "Name length can't be more than 30.")]
        public string Name { get; set; }
        [Required]
        [Range(0.01, 0.99)]
        [Precision(3, 2)]
        public decimal DiscountPercent { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public List<Product> Products { get; } = new List<Product>();
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
