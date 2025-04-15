using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PortfolioApp.Server.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly string _contentRoot;
        private readonly string _webRoot;

        public FileStorageService(IWebHostEnvironment env, IConfiguration config)
        {
            _env = env;
            _config = config;
            _contentRoot = _env.ContentRootPath;
            _webRoot = _env.WebRootPath;
        }

        public async Task<string> SaveFileAsync(byte[] fileData, string fileName, string contentType)
        {
            try
            {
                // Determine folder based on content type
                string folder = contentType.StartsWith("image/") ? "images" : 
                                contentType.StartsWith("application/pdf") ? "documents" : "files";
                
                // Create directory if it doesn't exist
                string uploadPath = Path.Combine(_webRoot, "uploads", folder);
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                // Generate unique filename
                string fileExtension = Path.GetExtension(fileName);
                string uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                string filePath = Path.Combine(uploadPath, uniqueFileName);

                // Save file
                await File.WriteAllBytesAsync(filePath, fileData);

                // Return the relative URL
                return $"/uploads/{folder}/{uniqueFileName}";
            }
            catch (Exception ex)
            {
                // Log error here
                throw new Exception($"File upload failed: {ex.Message}");
            }
        }

        public Task DeleteFileAsync(string fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl))
                return Task.CompletedTask;

            try
            {
                // Convert URL to physical path
                string filePath = Path.Combine(_webRoot, fileUrl.TrimStart('/'));
                
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                // Log error here
                throw new Exception($"File deletion failed: {ex.Message}");
            }
        }
    }
}