using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace Books.Hub.Application.DTOs.Authors
{
    public class EditAuthorDTO
    {
        [Required]
        public int Id { get; set; }

        [MaxLength(30)]
        public string? Name { get; set; }


        [MaxLength(30)]
        public string? Nationality { get; set; }


        [MaxLength(300)]
        public string? Bio { get; set; }

        public bool? IsActive { get; set; }

        public IFormFile? AuthorImageFile { get; set; }

        [SwaggerSchema(ReadOnly = true)] // Hides it from Swagger input
        [JsonIgnore] // Prevents it from being included in API requests
        [BindNever] // Ensures it's never bound from a request
        public byte[]? AuthorImage { get; set; }


        [JsonPropertyName("Date Of Brith")]
        [SwaggerSchema(Description = "Enter the date in format yyyy-MM-dd")]
        public string? DateOfBrith { get; set; }
    }
}
