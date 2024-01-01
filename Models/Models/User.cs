using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Models.Models
{
    public class User : IdentityUser
    {
        [StringLength(40,
            MinimumLength = 3,
            ErrorMessage = "First name length must be between 3 and 40.")]
        public string? FirstName { get; set; }
        [StringLength(40,
            MinimumLength = 3,
            ErrorMessage = "Last name length must be between 3 and 40.")]
        public string? LastName { get; set; }
        public DateTime? Birthdate { get; set; }
        public string? ImgPath { get; set; }
        public ICollection<UserAddress> UserAddresses { get; } = new List<UserAddress>();
        public ICollection<UserPayment> UserPayments { get; } = new List<UserPayment>();
        // This attribute to make sure our server do not send more than 1 email per minute.
        public DateTime LastEmailDate { get; set; }
        // Reviews list for all reviews belong to the user
        public List<Review> Reviews { get; } = new();

        [Required]
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
