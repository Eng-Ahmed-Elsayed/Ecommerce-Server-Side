using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Models.Models
{
    public class ShippingOption
    {
        public Guid Id { get; set; }
        [Required]
        public string Method { get; set; }
        [Required]
        [Precision(8, 2)]
        public decimal Cost { get; set; }
        [Required]
        public string DeliveryTime { get; set; }


        [Required]
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
