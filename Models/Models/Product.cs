using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public Guid? InventoryId { get; set; }
        public decimal Price { get; set; }
        public Guid? DiscoutId { get; set; }
        public Discount? Discount { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }



    }
}
