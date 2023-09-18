using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? ImgPath { get; set; }


        //[NotMapped]
        //public List<IFormFile> categoryImage { get; set; }


        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }
        //[Required]
        //[MinLength(5)]
        //[MaxLength(75)]

    }
}
