using System.ComponentModel.DataAnnotations;
using Models.DataTransferObjects.Shared;

namespace Models.DataTransferObjects.Customer
{
    public class CartItemDto
    {
        public Guid? Id { get; set; }
        [Required]
        public Guid ShoppingCartId { get; set; }
        [Required]
        public Guid ProductId { get; set; }
        public ProductDto? Product { get; set; }
        [Required]
        [Range(1, 99999)]
        public int Quantity { get; set; }
        public decimal? Price { get; set; }

    }
}
