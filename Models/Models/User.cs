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


        [Required]
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
