using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace Models.DataTransferObjects.Shared
{
    public class DiscountDto
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(40,
            MinimumLength = 5,
            ErrorMessage = "Name length must be between 5 and 40.")]
        public string Name { get; set; }
        [Required]
        [Range(0.01, 0.99)]
        [Precision(3, 2)]
        public decimal DiscountPercent { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public List<ProductDto> Products { get; } = new List<ProductDto>();
        public List<ProductDto>? OtherProducts { get; set; }

        public static implicit operator DiscountDto(Discount v)
        {
            throw new NotImplementedException();
        }
    }
}
