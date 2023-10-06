using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class UserAddress
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
        [Required]
        [StringLength(80, ErrorMessage = "AddressLine1 length can't be more than 80.")]
        public string AddressLine1 { get; set; }

        [StringLength(80, ErrorMessage = "AddressLine1 length can't be more than 80.")]
        public string? AddressLine2 { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "City length can't be more than 40.")]
        public string City { get; set; }
        [Required]
        [StringLength(40, ErrorMessage = "State length can't be more than 40.")]
        public string State { get; set; }
        [Required]
        [StringLength(40, ErrorMessage = "Country length can't be more than 40.")]
        public string Country { get; set; }
        [Required]
        [MinLength(4)]
        [MaxLength(10)]
        public int PostalCode { get; set; }
        [Phone]
        [MinLength(4)]
        [MaxLength(25)]
        public string? Telephone { get; set; }
        [MinLength(4)]
        [MaxLength(25)]
        [Phone]
        public string Mobile { get; set; }


        [Required]
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
