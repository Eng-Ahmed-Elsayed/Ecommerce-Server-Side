﻿using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class Discount
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal DiscountPercent { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Product> Products { get; } = new List<Product>();

        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
