using System.ComponentModel.DataAnnotations;
using Models.Models;

namespace Models.DataTransferObjects.Auth
{
    public class UserDto
    {
        public string? Id { get; set; }
        [StringLength(20,
            MinimumLength = 3,
            ErrorMessage = "Username length must be between 3 and 40.")]
        public string? UserName { get; set; }
        public string? NormalizedUserName { get; set; }
        [StringLength(40,
            MinimumLength = 3,
            ErrorMessage = "First name length must be between 3 and 40.")]
        public string? FirstName { get; set; }
        [StringLength(40,
            MinimumLength = 3,
            ErrorMessage = "Last name length must be between 3 and 40.")]
        public string? LastName { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [EmailAddress]
        public string? NormalizedEmail { get; set; }
        public bool? EmailConfirmed { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
        public string? PhoneNumberConfirmed { get; set; }
        public DateTime? Birthdate { get; set; }
        public string? ImgPath { get; set; }
        public bool? TwoFactorEnabled { get; set; }
        public bool? LockoutEnabled { get; set; }
        public int? AccessFailedCount { get; set; }

        public List<Review>? Reviews { get; set; }




    }
}
