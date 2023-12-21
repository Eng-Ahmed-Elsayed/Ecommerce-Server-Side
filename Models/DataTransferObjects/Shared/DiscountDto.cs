﻿using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

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
        [StringLength(6,
            MinimumLength = 6,
            ErrorMessage = "Code length must be 6.")]
        public string? Code { get; set; }
        [Required]
        [Range(0.01, 0.99)]
        [Precision(3, 2)]
        public decimal DiscountPercent { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public List<ProductDto>? Products { get; set; }
        public List<ProductDto>? OtherProducts { get; set; }


    }
}
