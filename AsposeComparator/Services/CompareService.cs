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
        private readonly IFileService _fileService;

        public CompareService(IColorComparator colorComparator, IGeometryService geometryService, IFileService fileService)
        {
            _colorComparator = colorComparator;
            _geometryService = geometryService;
            _fileService = fileService;
        }
        
        public CompareResponse CompareImages(string fileName1, string fileName2, int tolerance = 0, int maxDifferences = 0)
        {
            var filePath1 = _fileService.GetPath(fileName1);
            var filePath2 = _fileService.GetPath(fileName2);

            var differences = new List<Point>();

            var resultFileName = Guid.NewGuid().ToString() + Path.GetExtension(fileName1);
            var resultPath = _fileService.GetPath(resultFileName);

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

                var shift = GetImageShift(bitmap1, bitmap2);
                var x1 = shift.X < 0 ? 0 - shift.X : 0;
                var x2 = shift.X < 0 ? width : width - shift.X;
                var y1 = shift.Y < 0 ? 0 - shift.Y : 0;
                var y2 = shift.Y < 0 ? height : height - shift.Y;

                for (int i = x1; i < x2; i++)
                {
                    for (int j = y1; j < y2; j++)
                    {
                        var pixel1 = bitmap1.GetPixel(i, j);
                        var pixel2 = bitmap2.GetPixel(i + shift.X, j + shift.Y);
                        if (!_colorComparator.IsEqual(pixel1, pixel2, tolerance))
                        {
                            differences.Add(new Point(i, j));
                        }
                    }
                }

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
            return await Task.FromResult(CompareImages(fileName1, fileName2, tolerance, maxDifferences));
        }

        public Point GetImageShift(Bitmap bitmap1, Bitmap bitmap2)
        {
            var width = bitmap1.Width;
            var height = bitmap1.Height;
            var shiftPoint = new Point();
            var minErrorSum = 0.0;

            for (int m = -2; m < 3; m++) // shift by x
            {
                for (int n = -2; n < 3; n++) // shift by y
                {
                    var errorSum = 0.0;
                    var x1 = m < 0 ? 0 - m : 0;
                    var x2 = m < 0 ? width : width - m;
                    var y1 = n < 0 ? 0 - n : 0;
                    var y2 = n < 0 ? height : height - n;

                    for (int i = x1; i < x2; i++)
                    {
                        for (int j = y1; j < y2; j++)
                        {
                            var pixel1 = bitmap1.GetPixel(i, j);
                            var pixel2 = bitmap2.GetPixel(i + m, j + n);

                            errorSum += _colorComparator.GetDistance(pixel1, pixel2);
                        }
                    }

                    if (m == -2 && n == -2)
                    {
                        shiftPoint = new Point(m, n);
                        minErrorSum = errorSum;
                    } 
                    else if (minErrorSum > errorSum)
                    {
                        minErrorSum = errorSum;
                        shiftPoint = new Point(m, n);
                    }
                }
            }

            return shiftPoint;
        }

        public void SetColorComparator(IColorComparator colorComparator)
        {
            _colorComparator = colorComparator;
        }
    }
}
