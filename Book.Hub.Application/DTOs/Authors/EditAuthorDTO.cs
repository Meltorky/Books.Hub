using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;


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

        public IFormFile? AuthorImageFile { get; set; }

        [JsonPropertyName("Date Of Brith")]
        [SwaggerSchema(Description = "Enter the date in format yyyy-MM-dd")]
        public DateOnly DateOfBrith { get; set; }
    }
}
