using AsposeComparator.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AsposeComparator.Interfaces
{
    public interface IFileService
    {
        public FileStream GetFileStream(string fileName);
        Task<FileInfoResponse> SaveFile(IFormFile file);
        string GetContentType(string fileName);
    }
}
