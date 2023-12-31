﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Models.Models
{
    public class CartItem
    {
        public Guid Id { get; set; }
        public Guid ShoppingCartId { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; }
        [Required]
        [Range(1, 99999)]
        public int Quantity { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Precision(18, 2)]
        public decimal? Price
        {
            get
            {
                return Product.Price * Quantity;
            }
        }

        [Required]
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }

    }
}
