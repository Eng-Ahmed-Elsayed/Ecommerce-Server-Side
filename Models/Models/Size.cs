using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class Size
    {
        public Guid Id { get; set; }
        [StringLength(20)]
        [Required]
        public string Name { get; set; }
    }
}
