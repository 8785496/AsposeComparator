using NUnit.Framework;
using System.Drawing;
using AsposeComparator.Services;
using AsposeComparator.Models;

namespace AsposeComparatorTests
{
    class CompareServiceTests
    {
        [Test]
        public void ShiftImageRgbReturn1and2()
        {
            var compareService = new CompareService(new RgbColorComparator(), new GeometryService(), new FileService());

            using var bitmap1 = new Bitmap(10, 10);
            using var bitmap2 = new Bitmap(10, 10);
            var brush = new SolidBrush(Color.Black);

            using (var graphics1 = Graphics.FromImage(bitmap1))
            {
                graphics1.Clear(Color.White);
                graphics1.FillRectangle(brush, 2, 2, 5, 5);
            }

            using (var graphics2 = Graphics.FromImage(bitmap2))
            {
                graphics2.Clear(Color.White);
                graphics2.FillRectangle(brush, 3, 4, 5, 5);
            }

            var shift = compareService.GetImageShift(bitmap1, bitmap2);

            Assert.AreEqual(shift.X, 1);
            Assert.AreEqual(shift.Y, 2);
        }

        [Test]
        public void ShiftImageHsvReturn1and2()
        {
            var compareService = new CompareService(new HsvColorComparator(), new GeometryService(), new FileService());

            using var bitmap1 = new Bitmap(10, 10);
            using var bitmap2 = new Bitmap(10, 10);
            var brush = new SolidBrush(Color.Black);

            using (var graphics1 = Graphics.FromImage(bitmap1))
            {
                graphics1.Clear(Color.White);
                graphics1.FillRectangle(brush, 2, 2, 5, 5);
            }

            using (var graphics2 = Graphics.FromImage(bitmap2))
            {
                graphics2.Clear(Color.White);
                graphics2.FillRectangle(brush, 3, 4, 5, 5);
            }

            var shift = compareService.GetImageShift(bitmap1, bitmap2);

            Assert.AreEqual(shift.X, 1);
            Assert.AreEqual(shift.Y, 2);
        }

        [Test]
        public void ShiftImageArgbReturn1and2()
        {
            var compareService = new CompareService(new ArgbColorComparator(), new GeometryService(), new FileService());

            using var bitmap1 = new Bitmap(10, 10);
            using var bitmap2 = new Bitmap(10, 10);
            var brush = new SolidBrush(Color.Black);

            using (var graphics1 = Graphics.FromImage(bitmap1))
            {
                graphics1.Clear(Color.White);
                graphics1.FillRectangle(brush, 2, 2, 5, 5);
            }

            using (var graphics2 = Graphics.FromImage(bitmap2))
            {
                graphics2.Clear(Color.White);
                graphics2.FillRectangle(brush, 3, 4, 5, 5);
            }

            var shift = compareService.GetImageShift(bitmap1, bitmap2);

            Assert.AreEqual(shift.X, 1);
            Assert.AreEqual(shift.Y, 2);
        }

        [Test]
        public void CompareImagesReturn2Diff()
        {
            var fileService = new FileService();
            var compareService = new CompareService(new RgbColorComparator(), new GeometryService(), fileService);

            var result = compareService.CompareImages("test1.png", "test2.png", 0, 0);
            var path = fileService.GetPath(result.ResultFileName);

            using var bitmap = new Bitmap(path);
            var pixel1 = bitmap.GetPixel(60, 0);
            var pixel2 = bitmap.GetPixel(90, 20);
            var pixel3 = bitmap.GetPixel(10, 60);
            var pixel4 = bitmap.GetPixel(30, 90);
            var red = Color.FromArgb(255, 0, 0);

            Assert.AreEqual(pixel1, red);
            Assert.AreEqual(pixel2, red);
            Assert.AreEqual(pixel3, red);
            Assert.AreEqual(pixel4, red);
        }

        [Test]
        public void CompareImagesReturn1Diff()
        {
            var fileService = new FileService();
            var compareService = new CompareService(new RgbColorComparator(), new GeometryService(), fileService);

            var result = compareService.CompareImages("test1.png", "test2.png", 51, 0);
            var path = fileService.GetPath(result.ResultFileName);

            using var bitmap = new Bitmap(path);
            var pixel1 = bitmap.GetPixel(60, 0);
            var pixel2 = bitmap.GetPixel(90, 20);
            var pixel3 = bitmap.GetPixel(10, 60);
            var pixel4 = bitmap.GetPixel(30, 90);
            var red = Color.FromArgb(255, 0, 0);
            var black = Color.FromArgb(0, 0, 0);
            var white = Color.FromArgb(255, 255, 255);

            Assert.AreEqual(pixel1, black);
            Assert.AreEqual(pixel2, white);
            Assert.AreEqual(pixel3, red);
            Assert.AreEqual(pixel4, red);
        }
    }
}
