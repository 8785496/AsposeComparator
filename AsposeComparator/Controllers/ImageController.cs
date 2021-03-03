using AsposeComparator.Interfaces;
using AsposeComparator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsposeComparator.Controllers
{
    public class ImageController : Controller
    {
        private readonly ICompareService _compareService;
        private readonly ILogger<ImageController> _logger;

        public ImageController(ICompareService compareService, ILogger<ImageController> logger)
        {
            _compareService = compareService;
            _logger = logger;
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

            try
            {
                return Ok(await _compareService.CompareImagesAsync(fileName1, fileName2, tolerance, maxDifferences));
            }
            catch (RecursionException e)
            {
                _logger.LogError(e.StackTrace + $"\n\nfName1={fileName1}, fName2={fileName2}, tolerance={tolerance}, max={maxDifferences}, model={colorModel}");
                return Ok(new CompareResponse { Message = e.Message, IsSuccess = false });
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace + $"\n\nfName1={fileName1}, fName2={fileName2}, tolerance={tolerance}, max={maxDifferences}, model={colorModel}");
                return Ok(new CompareResponse { Message = "Server error", IsSuccess = false });
            }
        }
    }
}
