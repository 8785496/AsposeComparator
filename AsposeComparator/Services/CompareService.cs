using AsposeComparator.Interfaces;
using AsposeComparator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace AsposeComparator.Services
{
    public class CompareService : ICompareService
    {
        private readonly IColorComparator _colorComparator;
        
        public CompareService(IColorComparator colorComparator)
        {
            _colorComparator = colorComparator;
        }
        
        public CompareResponse CompareImages(string fileName1, string fileName2, int tolerance = 0, int maxDifferences = 0)
        {
            var filePath1 = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", fileName1);
            var filePath2 = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", fileName2);
            
            var isEquals = true;
            var differences = new List<Point>();

            var resultFileName = Guid.NewGuid().ToString() + ".png";
            var resultPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", resultFileName);

            using (var bitmap1 = new Bitmap(filePath1))
            using (var bitmap2 = new Bitmap(filePath2))
            {
                var width = bitmap1.Width;
                var height = bitmap1.Height;


                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        var pixel1 = bitmap1.GetPixel(i, j);
                        var pixel2 = bitmap2.GetPixel(i, j);
                        if (!_colorComparator.IsEqual(pixel1, pixel2, tolerance))
                        {
                            isEquals = false;
                            differences.Add(new Point(i, j));
                        }
                    }
                }

                var color = Color.FromArgb(255, 0, 0);
                foreach (var difference in differences)
                {
                    bitmap1.SetPixel(difference.X, difference.Y, color);
                }
                bitmap1.Save(resultPath);
            }

            return new CompareResponse
            {
                FileName1 = fileName1,
                FileName2 = fileName2,
                IsEquals = isEquals,
                ResultFileName = resultFileName
            };
            
        }

        private static bool IsEqualColors(Color color1, Color color2)
        {
            return color1.R == color2.R && color1.G == color2.G && color1.B == color2.B;
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
    }
}
