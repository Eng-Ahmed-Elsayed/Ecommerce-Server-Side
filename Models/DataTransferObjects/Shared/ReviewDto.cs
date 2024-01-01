using System.ComponentModel.DataAnnotations;
using Models.DataTransferObjects.Auth;

namespace Models.DataTransferObjects.Shared
{
    public class ReviewDto
    {
        public Guid Id { get; set; }
        [Required]
        public Guid ProductId { get; set; }
        public ProductDto? Product { get; set; }
        public string? UserId { get; set; }
        public UserDto? User { get; set; }
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
        [Required]
        [StringLength(80,
            MinimumLength = 5,
            ErrorMessage = "Title length must be between 5 and 80.")]
        public string Title { get; set; }
        [Required]
        [StringLength(400,
            MinimumLength = 5,
            ErrorMessage = "Comment length must be between 5 and 400.")]
        public string Comment { get; set; } // The comment given by the user
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
