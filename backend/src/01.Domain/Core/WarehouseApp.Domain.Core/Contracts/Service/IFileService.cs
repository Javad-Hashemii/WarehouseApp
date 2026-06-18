using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Contracts.Service
{
    public interface IFileService
    {
        void Delete(string fileName);
        string Upload(IFormFile file, string folder);
        string ReplaceFile(string? currentFilePath, IFormFile newFile, string folder);
        bool BeValidSize(IFormFile file);
        bool HaveValidExtension(IFormFile file);
    }
}
