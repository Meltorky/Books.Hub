using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Application.Constants
{
    public class BookFileOptions
    {
        public const string MaxSizeInMB = "1";
        public const string AllowedExtentions = ".pdf";
        public const string ErrorMessage = $"Only accept {AllowedExtentions} with max size of {MaxSizeInMB}MB";
    }
}
