using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DhofarAppApi.Services
{
    public class FileServices
    {
        private readonly long _maxFileSizeBytes;

        public FileServices(long maxFileSizeBytes = 104857600)
        {
            _maxFileSizeBytes = maxFileSizeBytes;
        }

        public async Task<string> SaveFileToServerAndDatabase(IFormFile file, string fileName)
        {
            var relativeFilePath = $"uploads/{fileName}"; // Constructing the relative file path
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), relativeFilePath);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            if (IsImage(file.FileName))
            {
                await ResizeAndSaveImage(file, filePath);
            }
            else if (IsVideo(file.FileName) && file.Length < _maxFileSizeBytes)
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            else
            {
                throw new InvalidOperationException($"File size exceeds the maximum limit of {_maxFileSizeBytes} bytes or file format is not supported.");
            }

            return relativeFilePath; // Returning only the relative file path
        
    }

        private async Task ResizeAndSaveImage(IFormFile file, string filePath)
        {
            using var image = Image.Load(file.OpenReadStream());
            const int width = 800;
            const int height = 600;
            image.Mutate(x => x.Resize(width, height));
            var encoder = new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder { Quality = 70 };
            await using var outputStream = new FileStream(filePath, FileMode.Create);
            await image.SaveAsync(outputStream, encoder);
        }

        private bool IsImage(string fileName)
        {
            var imageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            return imageExtensions.Any(ext => fileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
        }

        private bool IsVideo(string fileName)
        {
            var videoExtensions = new[] { ".mp4", ".avi", ".mov", ".wmv", ".flv" };
            return videoExtensions.Any(ext => fileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
        }
    }
}
