using System.ComponentModel.DataAnnotations;

namespace Models.DataTransferObjects.Shared
{
    public class ShippingOptionDto
    {
        public Guid? Id { get; set; }
        [Required]
        public string Method { get; set; }
        [Required]
        public decimal Cost { get; set; }
        [Required]
        public string DeliveryTime { get; set; }

    }
}
