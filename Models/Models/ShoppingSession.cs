using Microsoft.EntityFrameworkCore;

namespace Models.Models
{
    public class ShoppingSession
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        [Precision(18, 2)]
        public decimal Total { get; set; }


        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }

    }
}
