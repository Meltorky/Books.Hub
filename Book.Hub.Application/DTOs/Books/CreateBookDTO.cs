using Books.Hub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace Books.Hub.Application.DTOs.Books
{
    public class CreateBookDTO
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
        [JsonPropertyName("PublishedDate")]
        [SwaggerSchema(Description = "Enter the date in format yyyy-MM-dd")]
        public string PublishedDate { get; set; } = string.Empty;

        [Required]
        public int PageCount { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public bool IsAvailable { get; set; }

        [Required]
        public int TotalCopiesSold { get; set; }

        public IFormFile? BookCoverFile { get; set; }

        [SwaggerSchema(ReadOnly = true)] // Hides it from Swagger input
        [JsonIgnore] // Prevents it from being included in API requests
        [BindNever] // Ensures it's never bound from a request
        public byte[]? BookCover { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [Required]
        public List<int> BookCategoryIDs { get; set; } = new List<int>();
    }
}
