using System.ComponentModel.DataAnnotations;

namespace Models.DataTransferObjects.Shared
{
    public class CategoryDto
    {
        public Guid? Id { get; set; }
        [Required]
        [StringLength(40,
            MinimumLength = 5,
            ErrorMessage = "Name length must be between 5 and 40.")]
        public string? Name { get; set; }
        [Required]
        [StringLength(100,
            MinimumLength = 2,
            ErrorMessage = "Description length must be between 2 and 100.")]
        public string? Description { get; set; }
        public string? ImgPath { get; set; }


    }
}
