using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Models
{
    public class Inventory
    {
        public Guid Id { get; set; }
        [Required]
        [Range(0, 999999)]
        public int Quantity { get; set; }
        public ICollection<Product> Products { get; } = new List<Product>();
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Status
        {
            get
            {
                if (Quantity >= 15) return "IN STOCK";
                else if (Quantity < 15 & Quantity > 0) return "LOW STOCK";
                else return "OUT OF STOCK";
            }
        }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
