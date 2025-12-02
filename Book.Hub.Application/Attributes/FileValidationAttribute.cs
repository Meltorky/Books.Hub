using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Application.Attributes
{
    public class FileValidationAttribute : ValidationAttribute
    {
        private readonly string _maxSizeInMB;
        private readonly string _allowedExtentions;
        public FileValidationAttribute(string maxSizeInMB, string allowedExtentions)
        {
            _maxSizeInMB = maxSizeInMB;
            _allowedExtentions = allowedExtentions;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var imageFile = value as IFormFile;

            if (imageFile != null)
            {
                if (imageFile.Length > Convert.ToInt32(_maxSizeInMB) * 1048576 ||
                    !_allowedExtentions.Contains(Path.GetExtension(imageFile.FileName).ToLowerInvariant()))
                {
                    return new ValidationResult($"Only accept file with extentions {_allowedExtentions} and max size of {_maxSizeInMB} MB");
                }
            }

            return ValidationResult.Success;
        }
    }
}
