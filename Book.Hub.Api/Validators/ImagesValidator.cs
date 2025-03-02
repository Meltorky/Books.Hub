using Books.Hub.Application.Options;
using Microsoft.Extensions.Options;

namespace Books.Hub.Api.Validators
{
    public static class ImagesValidator
    {
        public static bool UploadedImagesValidator(IFormFile file, ImagesOptions options)
        {
            bool isAllowed = false;

            if (file != null
                && options.AllowedExtentions.Contains(Path.GetExtension(file.FileName).ToLower())
                && file.Length <= options.MaxSizeAllowedInBytes)
            {
                isAllowed = true;
            }

            return isAllowed;
        }
    }
}
