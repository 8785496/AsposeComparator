using AsposeComparator.Interfaces;
using AsposeComparator.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AsposeComparator.Services
{
    public class FileService : IFileService
    {
        public FileStream GetFileStream(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", fileName);
            return File.OpenRead(filePath);
        }

        public async Task<FileInfoResponse> SaveFile(IFormFile file)
        {
            if (file.Length > 0)
            {
                var rootFolder = Directory.GetCurrentDirectory();
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(rootFolder, "Uploads", fileName);
                using (var stream = File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                }
                return new FileInfoResponse
                { 
                    FileName = fileName
                };
            }
            throw new Exception("File not saved");
        }

        public string GetContentType(string fileName)
        {
            var ext = Path.GetExtension(fileName) ?? "";
            if (ext.ToLower().Contains("jpg") || (ext.ToLower().Contains("jpeg")))
            {
                return "image/jpeg";
            }
            if (ext.ToLower().Contains("png"))
            {
                return "image/png";
            }
            if (ext.ToLower().Contains("bmp"))
            {
                return "image/bmp";
            }
            return "image/*";
        }
    }
}
