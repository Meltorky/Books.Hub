using Books.Hub.Application.Interfaces.IServices.Comman;
using Imagekit.Sdk;
using Microsoft.AspNetCore.Http;


namespace Books.Hub.Application.Services.Comman
{
    public class ImageUploadService : IImageUploadService
    {
        public async Task<string> UploadAsync(IFormFile file, string fileName, bool isImage)
        {
            // Hardcoded Credentials (as requested)
            const string PRIVATE_KEY = "private_BHj6XebP54h9zpSbDuNNjSar9tM="; 
            const string PUBLIC_KEY = "public_Rgc4aFb0s/hjDySl1pqQsO9RLEg=";   
            const string URL_ENDPOINT = "https://ik.imagekit.io/meltorky1155";

            if (file == null || file.Length == 0)
                throw new ArgumentException("File is required.");

            if (string.IsNullOrEmpty(fileName))
                fileName = file.FileName;

            // Convert IFormFile to byte[] (using MemoryStream is fine for smaller files)
            byte[] fileBytes;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            // 3. Initialize ImageKit Client
            var imagekit = new ImagekitClient(
                publicKey: PUBLIC_KEY,
                privateKey: PRIVATE_KEY,
                urlEndPoint: URL_ENDPOINT
            );

            // 4. Create the Upload Request
            var request = new FileCreateRequest
            {
                file = fileBytes,
                fileName = fileName,
                folder = isImage ? "/images/" : "/books/", // Ensure folder starts and ends with /
                useUniqueFileName = true // ensures unique names
            };

            // 5. Upload and Handle Response
            var result = await imagekit.UploadAsync(request);

            // Check for server-side errors
            if (result is null || string.IsNullOrEmpty(result.url))
                throw new Exception("Upload failed. No URL returned from ImageKit.");

            return result.url; // Return the uploaded file URL
        }
    }
}
