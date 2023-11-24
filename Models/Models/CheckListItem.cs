using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class CheckListItem
    {
        public Guid Id { get; set; }
        public Guid CheckListId { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; }

        [Required]
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
