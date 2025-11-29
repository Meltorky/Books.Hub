using System.ComponentModel.DataAnnotations;
using Books.Hub.Application.Identity;

namespace Books.Hub.Domain.Entities
{
    public class BookReview
    {

        [Required]
        public string UserId { get; set; } = default!;

        [Required]
        public int BookId { get; set; }

        [Range(1, 10)]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public string Comment { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        // Navigation properties
        public ApplicationUser User { get; set; } = null!;
        public Book Book { get; set; } = new();
    }

}
