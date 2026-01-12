using Books.Hub.Application.Attributes;
using Books.Hub.Application.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace Books.Hub.Application.DTOs.Authors
{
    public class EditAuthorDTO
    {
        [Required]
        public int Id { get; set; }

        [MaxLength(30)]
        public string Name { get; set; } = string.Empty;


        [MaxLength(30)]
        public string Nationality { get; set; } = string.Empty;


        [MaxLength(300)]
        public string Bio { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        [FileValidation(ImageFileOptions.MaxSizeInMB, ImageFileOptions.AllowedExtentions)]
        [SwaggerSchema(Description = ImageFileOptions.ErrorMessage)]
        public IFormFile? AuthorImageFile { get; set; }


        [SwaggerSchema(ReadOnly = true)] // Hides it from Swagger input
        [JsonIgnore] // Prevents it from being included in API requests
        [BindNever] // Ensures it's never bound from a request
        public string? AuthorImageURL { get; set; }

        [SwaggerSchema(ReadOnly = true)] // Hides it from Swagger input
        [JsonIgnore] // Prevents it from being included in API requests
        [BindNever] // Ensures it's never bound from a request
        public string? AuthorImageId { get; set; }

        [JsonPropertyName("Date Of Brith")]
        [SwaggerSchema(Description = "Enter the date in format yyyy-MM-dd")]
        public DateOnly DateOfBrith { get; set; }
    }
}
