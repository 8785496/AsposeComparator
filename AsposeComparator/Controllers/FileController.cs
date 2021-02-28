using AsposeComparator.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AsposeComparator.Controllers
{
    public class FileController : Controller
    {
        private readonly IFileService _fileService;
        
        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }
        
        /// <summary>
        /// Returns image file
        /// </summary>
        public IActionResult Preview(string fileName)
        {
            return File(_fileService.GetFileStream(fileName), _fileService.GetContentType(fileName));
        }

        /// <summary>
        /// Saves an image on the server
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var reg = new Regex(@"image\/(png|jpg|jpeg|bmp)", RegexOptions.IgnoreCase);
            if (!reg.Match(file.ContentType).Success)
            {
                return BadRequest(new { error = "Invalid file format" });
            }
            try
            {
                var fileInfo = await _fileService.SaveFile(file);
                return Ok(fileInfo);
            }
            catch (Exception)
            {
                return BadRequest(new { error = "File not uploaded" });
            }
        }
    }
}
