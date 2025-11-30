using Books.Hub.Application.DTOs.Books;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Books.Hub.Application.DTOs.Authors
{
    public class AuthorDTO
    {
        public int Id { get; set; }

        [MaxLength(30)]
        [Required]
        public string Name { get; set; } = string.Empty;


        [Required]
        [MaxLength(30)]
        public string Nationality { get; set; } = string.Empty;


        [Required]
        [MaxLength(300)]
        public string Bio { get; set; } = string.Empty;


        [Required]
        public bool IsActive { get; set; }

        public byte[]? AuthorImage { get; set; }

        public int BooksSold { get; set; }
        public int SubscribersNumber { get; set; }

        [Required]
        [JsonPropertyName("Date Of Brith")]
        [SwaggerSchema(Description = "Enter the date in format yyyy-MM-dd")]
        public DateOnly DateOfBrith { get; set; }

        public string? ApplicationAuthorId { get; set; }

        public List<BookDTO> Books { get; set; } = new List<BookDTO>();
    }
}
