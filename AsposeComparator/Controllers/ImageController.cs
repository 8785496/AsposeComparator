using AsposeComparator.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsposeComparator.Controllers
{
    public class ImageController : Controller
    {
        private readonly ICompareService _compareService;

        public ImageController(ICompareService compareService)
        {
            _compareService = compareService;
        }

        public IActionResult CompareImages(string fileName1, string fileName2)
        {
            return Ok(_compareService.CompareImages(fileName1, fileName2));
        }
    }
}
