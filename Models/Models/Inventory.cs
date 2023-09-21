using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class Inventory
    {
        public Guid Id { get; set; }
        [Required]
        [Range(0, 999999)]
        public double Quantity { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
