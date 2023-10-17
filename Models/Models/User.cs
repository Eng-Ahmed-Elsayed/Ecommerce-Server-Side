using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Models.Models
{
    public class User : IdentityUser
    {
        [StringLength(40, ErrorMessage = "First Name length can't be more than 40.")]
        public string? FirstName { get; set; }
        [StringLength(40, ErrorMessage = "Last Name length can't be more than 40.")]
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
