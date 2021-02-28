using AsposeComparator.Interfaces;
using AsposeComparator.Models;
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

        /// <summary>
        /// Returns the filename of the comparison results
        /// </summary>
        public async Task<IActionResult> CompareImages(string fileName1, string fileName2, int tolerance = 0, int maxDifferences = 0, ColorModelEnum colorModel = ColorModelEnum.Rgb)
        {
            if (colorModel == ColorModelEnum.Argb)
            {
                _compareService.SetColorComparator(new ArgbColorComparator());
            }
            else if (colorModel == ColorModelEnum.Hsv)
            {
                _compareService.SetColorComparator(new HsvColorComparator());
            }
            
            return Ok(await _compareService.CompareImagesAsync(fileName1, fileName2, tolerance, maxDifferences));
        }
    }
}
