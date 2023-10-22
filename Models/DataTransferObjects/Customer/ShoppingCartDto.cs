using Microsoft.EntityFrameworkCore;

namespace Models.DataTransferObjects.Customer
{
    public class ShoppingCartDto
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public List<CartItemDto>? CartItems { get; set; }
        [Precision(18, 2)]
        public decimal Total { get; set; }
    }
}
