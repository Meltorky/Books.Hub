using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Books.Hub.Application.Identity;

namespace Books.Hub.Domain.Entities
{
    public class BookReview
    {
        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = default!;

        [ForeignKey(nameof(Book))]
        public int BookId { get; set; }

        [Range(1, 10)]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public string Comment { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        // Navigation properties
        public ApplicationUser? User { get; set; }
        public Book? Book { get; set; }
    }

}
