using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;
using AsposeComparator.Services;

namespace AsposeComparatorTests
{
    class GeometryServiseTests
    {
        [Test]
        public void GetRectanglesTest()
        {
            var geometryService = new GeometryService();
            var points = new List<Point>
            {
                new Point(2, 1),
                new Point(2, 2),
                new Point(3, 2),
                new Point(1, 3),
                new Point(2, 3),
                new Point(4, 4),
            };

            var rectangles = geometryService.GetRectangles(5, 5, points);
            Assert.AreEqual(rectangles.Count, 1);

            var rect = rectangles[0];
            Assert.AreEqual(rect.X, 1);
            Assert.AreEqual(rect.Y, 1);
            Assert.AreEqual(rect.Width, 3);
            Assert.AreEqual(rect.Height, 3);
        }
    }
}
