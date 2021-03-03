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
            var filePath = GetPath(fileName);
            return File.OpenRead(filePath);
        }

        public async Task<FileInfoResponse> SaveFile(IFormFile file)
        {
            if (file.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = GetPath(fileName);
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

        public string GetPath(string fileName)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "Uploads", fileName);
        }
    }
}
