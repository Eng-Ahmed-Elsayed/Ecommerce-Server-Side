using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace Models.DataTransferObjects.Shared
{
    public class ProductDto
    {
        public Guid? Id { get; set; }
        [Required]
        [StringLength(40,
            MinimumLength = 5,
            ErrorMessage = "Name length must be between 5 and 40.")]

        public string? Name { get; set; }
        [Required]
        [StringLength(100,
            MinimumLength = 2,
            ErrorMessage = "Description length must be between 2 and 100.")]

        public string? Description { get; set; }
        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        public string? ProductCode { get; set; }
        [Required]
        [MinLength(4)]
        [MaxLength(16)]
        public string? ProductSKU { get; set; }
        [Required]
        [Range(0.01, 999999.99)]
        [Precision(18, 2)]
        public decimal Price { get; set; }
        [Precision(2, 1)]
        [Range(0, 5)]
        public decimal Rating { get; set; }
        // Publish or draft 
        [Required]
        public string? Status { get; set; }
        [Required]
        public bool InStock { get; set; }
        [MaxLength(20)]
        public List<Tag>? Tags { get; set; }
        [MaxLength(20)]
        public List<Size>? Sizes { get; set; }
        [MaxLength(20)]
        public List<Color>? Colors { get; set; }
        [MaxLength(6)]
        public List<ProductImage>? ProductImages { get; set; }
        public Guid? CategoryId { get; set; }
        public CategoryDto? Category { get; set; }
        public Guid? InventoryId { get; set; }
        public InventoryDto? Inventory { get; set; }
        public List<DiscountDto>? Discounts { get; set; }
        // Reviews list
        public List<ReviewDto> Reviews { get; set; }
        [Precision(2, 1)]
        public decimal? AvgRating { get; set; }


        // Used to add or update inventory quantity to a product.
        public int Quantity { get; set; }


        // We will use this property to know if this product is in the user CheckList or not.
        public bool? IsInCheckList { get; set; }

    }
}
