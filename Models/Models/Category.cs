using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(80, ErrorMessage = "Name length can't be more than 80.")]
        public string Name { get; set; }
        [Required]
        [MinLength(20, ErrorMessage = "Description length can't be less than 20.")]
        public string Description { get; set; }
        public string? ImgPath { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }
        //[Required]
        //[MinLength(5)]
        //[MaxLength(75)]

    }
}
