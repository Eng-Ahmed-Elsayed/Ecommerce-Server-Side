using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class CategoryImage
    {
        [Key]
        public Guid CategoryImageId { get; set; }
        public Guid CategoryId { get; set; }
        public string ImageName { get; set; }
        public string ImagePath { get; set; }
    }
}
