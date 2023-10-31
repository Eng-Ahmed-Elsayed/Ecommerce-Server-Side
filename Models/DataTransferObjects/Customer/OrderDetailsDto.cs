using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Models.DataTransferObjects.Shared;

namespace Models.DataTransferObjects.Customer
{
    public class OrderDetailsDto
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        [Required]
        [Precision(18, 2)]
        public decimal Total { get; set; }
        public ICollection<OrderItemDto>? OrderItems { get; set; }
        [Required]
        public Guid ShippingOptionId { get; set; }
        public ShippingOptionDto? ShippingOption { get; set; }
        [Required]
        public Guid UserAddressId { get; set; }
        public UserAddressDto? UserAddress { get; set; }
        [Required]
        public Guid UserPaymentId { get; set; }
        public UserPaymentDto? UserPayment { get; set; }
    }
}
