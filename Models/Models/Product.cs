using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Models.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(80, ErrorMessage = "Name length can't be more than 80.")]
        public string Name { get; set; }
        [Required]
        [MinLength(20, ErrorMessage = "Description length can't be less than 20.")]

        public string Description { get; set; }
        [MinLength(4)]
        [MaxLength(30)]
        public string? ProductCode { get; set; }
        [Required]
        [MinLength(4)]
        [MaxLength(16)]
        public string ProductSKU { get; set; }
        [Required]
        [Range(0.01, 999999.99)]
        [Precision(18, 2)]
        public decimal Price { get; set; }
        [Precision(2, 1)]
        [Range(0, 5)]
        public decimal Rating { get; set; }
        // Publish or draft 
        [Required]
        public string Status { get; set; }
        [Required]
        public bool InStock { get; set; }
        [MaxLength(20)]
        public List<Tag> Tags { get; set; } = new();
        [MaxLength(20)]
        public List<Color> Colors { get; set; } = new();
        public List<ProductImage> ProductImages { get; set; } = new List<ProductImage>();


        public Guid? CategoryId { get; set; }
        public Category? Category { get; }
        public Guid? InventoryId { get; set; }
        public Inventory? Inventory { get; }
        public Guid? DiscoutId { get; set; }
        [Required]
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }



    }
}
