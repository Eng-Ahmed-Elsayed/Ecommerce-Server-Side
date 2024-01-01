using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class Review
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }
        public string UserId { get; set; }
        public User? User { get; set; }
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
        [Required]
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
