using System.ComponentModel.DataAnnotations;
using Models.DataTransferObjects.Auth;

namespace Models.DataTransferObjects.Shared
{
    public class UserAddressDto
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public UserDto? User { get; set; }
        [Required]
        [StringLength(40, ErrorMessage = "First Name length can't be more than 40.")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(40, ErrorMessage = "Last Name length can't be more than 40.")]
        public string LastName { get; set; }
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
    }
}
