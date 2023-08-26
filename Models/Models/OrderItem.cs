﻿using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid OrderDetailsId { get; set; }
        public OrderDetails OrderDetails { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}