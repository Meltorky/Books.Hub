using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Books.Hub.Application.DTOs.Books
{
    public class EditBookDTO
    {
        [Required]
        public int Id { get; set; }

        [MaxLength(80)]
        public string? Name { get; set; }

        
        [MaxLength(300)]
        public string? Description { get; set; }

        
        [MaxLength(30)]
        public string? Language { get; set; }

        
        [Range(1, 10)]
        public double? Rating { get; set; }

        
        [JsonPropertyName("PublishedDate")]
        [SwaggerSchema(Description = "Enter the date in format yyyy-MM-dd")]
        public DateOnly? PublishedDate { get; set; }

        
        public int? PageCount { get; set; }

        
        public double? Price { get; set; }

        
        public bool? IsAvailable { get; set; }

        
        public int? TotalCopiesSold { get; set; }

        public IFormFile? BookCoverFile { get; set; }

        [SwaggerSchema(ReadOnly = true)] // Hides it from Swagger input
        [JsonIgnore] // Prevents it from being included in API requests
        [BindNever] // Ensures it's never bound from a request
        public byte[]? BookCover { get; set; }

        
        public int? AuthorId { get; set; }

        
        public List<int>? BookCategoryIDs { get; set; }
    }
}
