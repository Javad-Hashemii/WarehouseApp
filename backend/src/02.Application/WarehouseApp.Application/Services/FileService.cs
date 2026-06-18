using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Configurations;
using WarehouseApp.Domain.Core.Contracts.Service;

namespace WarehouseApp.Application.Services
{
    public class FileService(SiteSettings siteSettings):IFileService
    {
        public void Delete(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return;

            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileName);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        public string Upload(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
                return null;

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", folder);

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            var filePath = Path.Combine(uploadsFolder, uniqueFileName);


            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return $"/Images/{folder}/{uniqueFileName}";
        }



        public string ReplaceFile(string? currentFilePath, IFormFile newFile, string folder)
        {
            if (newFile == null || newFile.Length == 0)
                return currentFilePath;

            if (!string.IsNullOrEmpty(currentFilePath))
                Delete(currentFilePath.TrimStart('/'));

            return Upload(newFile, folder);
        }

        public bool BeValidSize(IFormFile file)
        {
            var maxBytes = siteSettings.FileSettings.MaxFileSizeMB * 1024 * 1024;
            return file.Length <= maxBytes;
        }

        public bool HaveValidExtension(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLower();
            return siteSettings.FileSettings.AllowedExtensions.Contains(extension);
        }
    }
}
