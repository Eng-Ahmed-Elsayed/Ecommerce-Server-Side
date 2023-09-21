using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class Color
    {
        public Guid Id { get; set; }
        [StringLength(20)]
        public string Name { get; set; }

    }
}
