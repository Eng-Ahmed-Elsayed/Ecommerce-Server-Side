using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class CheckList
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public List<CheckListItem>? CheckListItems { get; } = new();


        [Required]
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
