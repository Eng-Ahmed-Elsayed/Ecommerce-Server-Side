using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Models.DataQuerying
{
    public class ProductParameters : QueryStringParameters
    {
        [StringLength(80, ErrorMessage = "Name length can't be more than 80.")]
        public string? Name { get; set; }
        // List of Inventory status => IN STOCK, LOW STOCK and OUT OF STOCK
        [MaxLength(3)]
        public List<string>? Availability { get; set; }

        // List of colors name
        [MaxLength(20)]
        public List<string>? Colors { get; set; }

        // List of sizes name
        [MaxLength(20)]
        public List<string>? Sizes { get; set; }

        [Range(0, 20000)]
        [Precision(18, 2)]
        public decimal? MinPrice { get; set; }
        [Range(0, 20000)]
        [Precision(18, 2)]
        public decimal? MaxPrice { get; set; }
        public bool ValidPriceRange => MinPrice != null && MaxPrice != null && MaxPrice >= MinPrice;

        [StringLength(40, ErrorMessage = "Category name length can't be more than 40.")]
        public string? Category { get; set; }

        public ProductParameters()
        {
            OrderBy = "name";
        }

    }


}
