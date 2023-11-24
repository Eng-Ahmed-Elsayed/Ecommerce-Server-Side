using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace Models.DataTransferObjects.Shared
{
    public class ProductDto
    {
        public Guid? Id { get; set; }
        [StringLength(80, ErrorMessage = "Name length can't be more than 80.")]
        public string? Name { get; set; }
        public string? Description { get; set; }
        [MinLength(4)]
        [MaxLength(30)]
        public string? ProductCode { get; set; }
        [MinLength(4)]
        [MaxLength(16)]
        public string? ProductSKU { get; set; }
        [Range(0.01, 999999.99)]
        [Precision(18, 2)]
        public decimal Price { get; set; }
        [Precision(2, 1)]
        [Range(0, 5)]
        public decimal? Rating { get; set; }
        // Publish or draft 
        public string? Status { get; set; }
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

        public Guid? DiscoutId { get; set; }
        public DiscountDto? Discount { get; set; }

        // Used to add or update inventory quantity to a product.
        public int Quantity { get; set; }

        // Used in get or get list actions.
        // If we did this from the model it will give us an error in the client
        // because the product will inculde the inventory then will inculde the product
        // and so on.
        public InventoryDto? Inventory { get; set; }

        // We will use this property to know if this product is in the user CheckList or not.
        public bool? IsInCheckList { get; set; }

    }
}
