using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Application.Interfaces.IServices.Comman
{
    public interface IImageUploadService
    {
        Task<string> UploadAsync(IFormFile file, string fileName, bool isImage);
    }
}
