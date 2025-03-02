using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Application.Options
{
    public class ImagesOptions
    {
        public int MaxSizeAllowedInBytes { get; set; } 
        public string AllowedExtentions { get; set; } = string.Empty;
    }
}
