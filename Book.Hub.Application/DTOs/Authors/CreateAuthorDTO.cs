using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations; // Add this line

namespace Books.Hub.Application.DTOs.Authors
{
    public class CreateAuthorDTO
    {
        [MaxLength(30)]
        [Required]
        public string Name { get; set; } = string.Empty;


        [Required]
        [MaxLength(30)]
        public string Nationality { get; set; } = string.Empty;


        [Required]
        [MaxLength(300)]
        public string Bio { get; set; } = string.Empty;
        

        public IFormFile? AuthorImageFile { get; set; }


        [SwaggerSchema(ReadOnly = true)] // Hides it from Swagger input
        [JsonIgnore] // Prevents it from being included in API requests
        [BindNever] // Ensures it's never bound from a request
        public byte[]? AuthorImage { get; set; }


        [Required]
        [JsonPropertyName("Date Of Brith")]
        [SwaggerSchema(Description = "Enter the date in format yyyy-MM-dd")]
        public DateOnly DateOfBrith { get; set; }
    }
}
