using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class UserAddress
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
        [Required]
        [StringLength(20,
            MinimumLength = 2,
            ErrorMessage = "First Name length must be between 2 and 20.")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(20,
            MinimumLength = 2,
            ErrorMessage = "Last Name length must be between 2 and 20.")]
        public string LastName { get; set; }
        [Required]
        [StringLength(80,
            MinimumLength = 2,
            ErrorMessage = "AddressLine1 length must be between 2 and 80.")]
        public string AddressLine1 { get; set; }

        [StringLength(80,
            MinimumLength = 2,
            ErrorMessage = "AddressLine2 length must be between 2 and 80.")]
        public string? AddressLine2 { get; set; }

        [Required]
        [StringLength(40,
            MinimumLength = 2,
            ErrorMessage = "City length must be between 2 and 40.")]
        public string City { get; set; }
        [Required]
        [StringLength(40,
            MinimumLength = 2,
            ErrorMessage = "State length must be between 2 and 40.")]
        public string State { get; set; }
        [Required]
        [StringLength(40,
            MinimumLength = 2,
            ErrorMessage = "Country length must be between 2 and 40.")]
        public string Country { get; set; }
        [Required]
        [MinLength(4)]
        [MaxLength(10)]
        public string PostalCode { get; set; }
        [Phone]
        [MinLength(4)]
        [MaxLength(25)]
        public string? Telephone { get; set; }
        [Required]
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
