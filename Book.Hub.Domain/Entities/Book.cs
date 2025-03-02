using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Domain.Entities
{
    public class Book : BaseEntity
    {
        [MaxLength(300)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(30)]
        public string Language { get; set; } = string.Empty;

        [Range(1, 10)]
        public double Rating { get; set; }
        public DateOnly PublishedDate { get; set; }
        public int PageCount { get; set; }
        public double Price { get; set; }
        public bool IsAvailable { get; set; }
        public int TotalCopiesSold { get; set; }
        public byte[]? BookCover { get; set; }

        // Navigation Properties

        [ForeignKey(nameof(Author))]
        public int AuthorId { get; set; }
        public Author Author { get; set; } = default!;

        public ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();

    }
}
