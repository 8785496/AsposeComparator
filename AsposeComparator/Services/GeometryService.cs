using AsposeComparator.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace AsposeComparator.Services
{
    public class GeometryService : IGeometryService
    {       
        public List<Rectangle> GetRectangles(int width, int height, List<Point> points, int maxResults = 0)
        {
            var usedPoints = new Dictionary<string, bool>();
            foreach (var point in points)
            {
                usedPoints.Add($"{point.X},{point.Y}", false);
            }

            var rectangles = new List<Rectangle>();
            foreach (var point in points)
            {
                if (usedPoints[$"{point.X},{point.Y}"])
                {
                    continue;
                }
                var rectangle = new Rectangle(point.X, point.Y, 1, 1);
                GetRectangle(ref rectangle, point, usedPoints, width, height);
                rectangles.Add(rectangle);
                if (maxResults > 0 && rectangles.Count >= maxResults)
                {
                    break;
                }
            }
            return rectangles;
        }

        public async Task<List<Rectangle>> GetRectanglesAsync(int width, int height, List<Point> points, int maxResults = 0)
        {
            var usedPoints = new Dictionary<string, bool>();
            foreach (var point in points)
            {
                usedPoints.Add($"{point.X},{point.Y}", false);
            }

            var rectangles = new List<Rectangle>();
            foreach (var point in points)
            {
                if (usedPoints[$"{point.X},{point.Y}"])
                {
                    continue;
                }
                var rectangle = new Rectangle(point.X, point.Y, 1, 1);
                GetRectangle(ref rectangle, point, usedPoints, width, height);
                rectangles.Add(rectangle);
                if (maxResults > 0 && rectangles.Count >= maxResults)
                {
                    break;
                }
            }
            return rectangles;
        }

        private static void GetRectangle(ref Rectangle rect, Point point, Dictionary<string, bool> usedPoints, int width, int height)
        {
            var x = point.X;
            var y = point.Y;
            usedPoints[$"{x},{y}"] = true;

            if (x + 1 < width && usedPoints.TryGetValue($"{x + 1},{y}", out bool value) && !value)
            {
                if (rect.X + rect.Width <= x + 1)
                {
                    rect.Width = x + 2 - rect.X;
                }
                var p1 = new Point(x + 1, y);
                GetRectangle(ref rect, p1, usedPoints, width, height);
            }

            if (x - 1 >= 0 && usedPoints.TryGetValue($"{x - 1},{y}", out value) && !value)
            {
                if (rect.X > x - 1)
                {
                    rect.Width = rect.Width + rect.X - (x - 1);
                    rect.X = x - 1;
                }
                var p2 = new Point(x - 1, y);
                GetRectangle(ref rect, p2, usedPoints, width, height);
            }

            if (y + 1 < height && usedPoints.TryGetValue($"{x},{y + 1}", out value) && !value)
            {
                if (rect.Y + rect.Height <= y + 1)
                {
                    rect.Height = y + 2 - rect.Y;
                }
                var p3 = new Point(x, y + 1);
                GetRectangle(ref rect, p3, usedPoints, width, height);
            }

            if (y - 1 >= 0 && usedPoints.TryGetValue($"{x},{y - 1}", out value) && !value)
            {
                if (rect.Y > y - 1)
                {
                    rect.Height = rect.Height + rect.Y - (y - 1);
                    rect.Y = y - 1;
                }
                var p4 = new Point(x, y - 1);
                GetRectangle(ref rect, p4, usedPoints, width, height);
            }
        }
    }
}
