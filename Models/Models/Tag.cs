﻿using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class Tag
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(20)]
        public string Name { get; set; }

    }
}
