using System.ComponentModel.DataAnnotations;

namespace Books.Hub.Application.DTOs.Categories
{
    public class CategoryDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(80)]
        public string Name { get; set; } = string.Empty;

        public int BooksNumber { get; set; }
    }
}
