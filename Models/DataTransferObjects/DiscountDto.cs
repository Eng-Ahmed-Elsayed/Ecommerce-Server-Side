using Models.Models;

namespace Models.DataTransferObjects
{
    public class DiscountDto
    {
        public Guid Id { get; set; }
        //[Required]
        //[StringLength(30, ErrorMessage = "Name length can't be more than 30.")]
        public string Name { get; set; }
        //[Required]
        //[Range(0.01, 0.99)]
        //[Precision(1, 2)]
        public decimal DiscountPercent { get; set; }
        //[Required]
        public bool IsActive { get; set; }
        public List<ProductDto> Products { get; } = new List<ProductDto>();
        public List<ProductDto>? OtherProducts { get; set; }

        public static implicit operator DiscountDto(Discount v)
        {
            throw new NotImplementedException();
        }
    }
}
