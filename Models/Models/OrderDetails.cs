using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Models.Models
{
    public class OrderDetails
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public ICollection<OrderItem>? OrderItems { get; } = new List<OrderItem>();

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Precision(18, 2)]
        public decimal? Total
        {
            get
            {
                decimal? total = 0;
                foreach (var item in OrderItems)
                {
                    total += item.Price;
                }
                return total;
            }
        }

        [StringLength(6,
            MinimumLength = 6,
            ErrorMessage = "Discount code length must be 6.")]
        public string? DiscountCode { get; set; }

        public Guid ShippingOptionId { get; set; }
        public ShippingOption? ShippingOption { get; }
        public Guid UserAddressId { get; set; }
        public UserAddress? UserAddress { get; }
        public Guid UserPaymentId { get; set; }
        public UserPayment? UserPayment { get; }


        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? CreatedAt { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? UpdatedAt { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
