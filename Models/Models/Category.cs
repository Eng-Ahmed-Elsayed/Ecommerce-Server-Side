using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(40,
            MinimumLength = 2,
            ErrorMessage = "Name length must be between 2 and 40.")]
        public string Name { get; set; }
        [Required]
        [StringLength(100,
            MinimumLength = 2,
            ErrorMessage = "Description length must be between 2 and 100.")]
        public string Description { get; set; }
        public string? ImgPath { get; set; }

        [Required]
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }
        //[Required]
        //[MinLength(5)]
        //[MaxLength(75)]

    }
}
