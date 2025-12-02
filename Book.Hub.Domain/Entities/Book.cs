using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Books.Hub.Domain.Entities
{
    public class Book : BaseEntity
    {
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(30)]
        public string Language { get; set; } = string.Empty;

        [Range(1, 10)]
        public double Rating { get; set; }
        public DateOnly PublishedDate { get; set; }
        public int PageCount { get; set; }
        public double Price { get; set; }
        public bool IsAvailable { get; set; } = true;
        public int TotalCopiesSold { get; set; }
        public string? BookCoverURL { get; set; }
        public string? BookFileURL { get; set; }

        // Navigation Properties

        [ForeignKey(nameof(Author))]
        public int AuthorId { get; set; }
        public Author Author { get; set; } = null!;

        public ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();
        public ICollection<UserBook> UserBooks { get; set; } = new List<UserBook>();
        public ICollection<BookReview> BookReviews { get; set; } = new List<BookReview>();
    }
}
