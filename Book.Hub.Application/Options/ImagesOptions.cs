using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Application.Options
{
    public class ImagesOptions
    {
        //[Range(1, 20, ErrorMessage = "MaxSizeMB must be between 1 and 20 MB.")]
        public int MaxSizeAllowedInBytes { get; set; } 
        public string AllowedExtentions { get; set; } = string.Empty;
    }
}
