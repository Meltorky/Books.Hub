using Books.Hub.Application.Attributes;
using Books.Hub.Application.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Books.Hub.Application.DTOs.Books
{
    public class FormBookDTO
    {
        [Required]
        [MaxLength(80)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(300)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [MaxLength(30)]
        public string Language { get; set; } = string.Empty;

        [Required]
        [Range(1, 10)]
        public double Rating { get; set; }

        [Required]
        [JsonPropertyName("Published Date")]
        [SwaggerSchema(Description = "Enter the date in format yyyy-MM-dd")]
        public DateOnly PublishedDate { get; set; }

        [Required]
        public int PageCount { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public int TotalCopiesSold { get; set; }


        [SwaggerSchema(Description = ImageFileOptions.ErrorMessage)]
        [FileValidation(ImageFileOptions.MaxSizeInMB, ImageFileOptions.AllowedExtentions)]
        public IFormFile? BookCover { get; set; }


        [SwaggerSchema(Description = BookFileOptions.ErrorMessage)]
        [FileValidation(BookFileOptions.MaxSizeInMB, BookFileOptions.AllowedExtentions)]
        public IFormFile? BookFile { get; set; }

        
        [Required]
        public int AuthorId { get; set; }

        [Required]
        [MinLength(1)]
        public List<int> BookCategoryIDs { get; set; } = new List<int>();


        [SwaggerSchema(ReadOnly = true)] // Hides it from Swagger input
        [JsonIgnore] // Prevents it from being included in API requests
        [BindNever] // Ensures it's never bound from a request
        public string? BookCoverURL { get; set; }

        [SwaggerSchema(ReadOnly = true)] // Hides it from Swagger input
        [JsonIgnore] // Prevents it from being included in API requests
        [BindNever] // Ensures it's never bound from a request
        public string? BookFileURL { get; set; }

        [SwaggerSchema(ReadOnly = true)] // Hides it from Swagger input
        [JsonIgnore] // Prevents it from being included in API requests
        [BindNever] // Ensures it's never bound from a request
        public string? BookCoverID { get; set; }

        [SwaggerSchema(ReadOnly = true)] // Hides it from Swagger input
        [JsonIgnore] // Prevents it from being included in API requests
        [BindNever] // Ensures it's never bound from a request
        public string? BookFileID { get; set; }
    }
}
