using AsposeComparator.Interfaces;
using AsposeComparator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;

namespace AsposeComparator.Services
{
    public class CompareService : ICompareService
    {
        private IColorComparator _colorComparator;
        private readonly IGeometryService _geometryService;
        
        public CompareService(IColorComparator colorComparator, IGeometryService geometryService)
        {
            _colorComparator = colorComparator;
            _geometryService = geometryService;
        }
        
        public CompareResponse CompareImages(string fileName1, string fileName2, int tolerance = 0, int maxDifferences = 0)
        {
            var filePath1 = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", fileName1);
            var filePath2 = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", fileName2);
            
            var differences = new List<Point>();

            var resultFileName = Guid.NewGuid().ToString() + Path.GetExtension(fileName1);
            var resultPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", resultFileName);

            using (var bitmap1 = new Bitmap(filePath1))
            using (var bitmap2 = new Bitmap(filePath2))
            {
                var width = bitmap1.Width;
                var height = bitmap1.Height;

                if (width != bitmap2.Width || height != bitmap2.Height)
                {
                    return new CompareResponse
                    {
                        IsSuccess = false,
                        Message = "Files have different sizes"
                    };
                }

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        var pixel1 = bitmap1.GetPixel(i, j);
                        var pixel2 = bitmap2.GetPixel(i, j);
                        if (!_colorComparator.IsEqual(pixel1, pixel2, tolerance))
                        {
                            differences.Add(new Point(i, j));
                        }
                    }
                }

                //code for debugging
                //foreach (var difference in differences)
                //{
                //    bitmap1.SetPixel(difference.X, difference.Y, Color.Blue);
                //}

                using (var graphics = Graphics.FromImage(bitmap1))
                {
                    var pen = new Pen(Color.Red);
                    var rectangles = _geometryService.GetRectangles(width, height, differences, maxDifferences);
                    foreach (var rect in rectangles)
                    {
                        graphics.DrawRectangle(pen, rect);
                    }
                }
                bitmap1.Save(resultPath);
            }

            return new CompareResponse
            {
                FileName1 = fileName1,
                FileName2 = fileName2,
                ResultFileName = resultFileName,
                IsSuccess = true
            };
            
        }

        public async Task<CompareResponse> CompareImagesAsync(string fileName1, string fileName2, int tolerance = 0, int maxDifferences = 0)
        {
            var filePath1 = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", fileName1);
            var filePath2 = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", fileName2);

            var differences = new List<Point>();

            var resultFileName = Guid.NewGuid().ToString() + Path.GetExtension(fileName1);
            var resultPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", resultFileName);

            using (var bitmap1 = new Bitmap(filePath1))
            using (var bitmap2 = new Bitmap(filePath2))
            {
                var width = bitmap1.Width;
                var height = bitmap1.Height;

                if (width != bitmap2.Width || height != bitmap2.Height)
                {
                    return new CompareResponse
                    {
                        IsSuccess = false,
                        Message = "Files have different sizes"
                    };
                }

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        var pixel1 = bitmap1.GetPixel(i, j);
                        var pixel2 = bitmap2.GetPixel(i, j);
                        if (!_colorComparator.IsEqual(pixel1, pixel2, tolerance))
                        {
                            differences.Add(new Point(i, j));
                        }
                    }
                }

                //code for debugging
                //foreach (var difference in differences)
                //{
                //    bitmap1.SetPixel(difference.X, difference.Y, Color.Blue);
                //}

                using (var graphics = Graphics.FromImage(bitmap1))
                {
                    var pen = new Pen(Color.Red);
                    var rectangles = await _geometryService.GetRectanglesAsync(width, height, differences, maxDifferences);
                    foreach (var rect in rectangles)
                    {
                        graphics.DrawRectangle(pen, rect);
                    }
                }
                bitmap1.Save(resultPath);
            }

            return new CompareResponse
            {
                FileName1 = fileName1,
                FileName2 = fileName2,
                ResultFileName = resultFileName,
                IsSuccess = true
            };

        }

        public void SetColorComparator(IColorComparator colorComparator)
        {
            _colorComparator = colorComparator;
        }
    }
}
