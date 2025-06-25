using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;

namespace Books.Hub.Application.DTOs.Auth
{
    public class RegisterDTO
    {
        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; } = string.Empty;


        [Required]
        [MaxLength(30)]
        public string LastName { get; set; } = string.Empty;


        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public string Email { get; set; } = string.Empty;


        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;


        [Required]
        [SwaggerSchema(Description = "Author or Subscriber")]
        public string RoleName { get; set; } = string.Empty;

    }
}
