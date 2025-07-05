using Books.Hub.Application.DTOs.Books;
using Books.Hub.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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


        [Required]
        [JsonPropertyName("Date Of Brith")]
        [SwaggerSchema(Description = "Enter the date in format yyyy-MM-dd")]
        public DateOnly DateOfBrith { get; set; }

        public string? ApplicationAuthorId { get; set; }

        public List<BookDTO> Books { get; set; } = new List<BookDTO>();
    }
}
