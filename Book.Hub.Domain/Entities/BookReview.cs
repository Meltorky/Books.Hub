using System.ComponentModel.DataAnnotations;
using Books.Hub.Application.Identity;

namespace Books.Hub.Domain.Entities
{
    public class BookReview
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public int BookId { get; set; }

        [Range(1, 10)]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public string Comment { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        // Navigation properties
        public ApplicationUser User { get; set; } = new();
        public Book Book { get; set; } = new();
    }

}
