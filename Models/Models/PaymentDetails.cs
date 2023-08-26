﻿using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class PaymentDetails
    {
        public Guid Id { get; set; }
        public Guid OrderDetailsId { get; set; }
        public int Amount { get; set; }
        public string Provider { get; set; }
        public string Status { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}