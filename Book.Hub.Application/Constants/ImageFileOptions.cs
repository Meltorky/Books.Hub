using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Application.Constants
{
    public class ImageFileOptions
    {
        public const string MaxSizeInMB = "1";
        public const string AllowedExtentions = ".jpg .png .jpeg";
        public const string ErrorMessage = $"Only accept {AllowedExtentions} with max size of {MaxSizeInMB}MB";
    }
}
