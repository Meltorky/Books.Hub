using Books.Hub.Application.DTOs.Categories;
using Books.Hub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Application.DTOs.Books
{
    public class BookDTO
    {
        public int Id { get; set; }

        [MaxLength(80)]
        public string Name { get; set; } = string.Empty;

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
        public string? BookCoverURL { get; set; }
        public string? BookFileURL { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public List<CategoryDTO> BookCategories { get; set; } = new List<CategoryDTO>();
    }
}
