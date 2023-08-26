using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class ShoppingSession
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public decimal Total { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }

    }
}
