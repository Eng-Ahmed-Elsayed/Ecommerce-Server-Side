using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class CartItem
    {
        public Guid Id { get; set; }
        public Guid ShoppingSessionId { get; set; }
        public Guid ProductId { get; set; }
        [Range(1, 9999)]
        public int Quantity { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }

    }
}
