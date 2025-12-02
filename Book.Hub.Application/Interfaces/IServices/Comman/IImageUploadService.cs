using Books.Hub.Application.DTOs.Common;
using Microsoft.AspNetCore.Http;

namespace Books.Hub.Application.Interfaces.IServices.Comman
{
    public interface IImageUploadService
    {
        Task<string> DeleteAsync(string fileId);
        Task<ImageKitResultDTO> UploadAsync(IFormFile file, string fileName, bool isImage);
    }
}
