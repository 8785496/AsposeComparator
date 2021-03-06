﻿using AsposeComparator.Interfaces;
using AsposeComparator.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace AsposeComparator.Services
{
    public class GeometryService : IGeometryService
    {
        /// <summary>
        /// Returns a list of rectangles that surround the differences.
        /// The points are grouped together using a recursive algorithm. If a point borders another point on the right, left, top, or bottom, then these points are grouped together. 
        /// The search for points continues until there are no bordering points or the border of the image is reached.
        /// Point group boundaries are recalculated when a new point is added to the group.
        /// </summary>
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
                GetRectangle(ref rectangle, 0, point, usedPoints, width, height);
                if (rectangle.Width * rectangle.Height >= 8)
                {
                    rectangles.Add(rectangle);
                }
                if (maxResults > 0 && rectangles.Count >= maxResults)
                {
                    break;
                }
            }
            return rectangles;
        }

        private static void GetRectangle(ref Rectangle rect, int counter, Point point, Dictionary<string, bool> usedPoints, int width, int height)
        {
            var x = point.X;
            var y = point.Y;
            usedPoints[$"{x},{y}"] = true;
            counter++;
            if (counter > 1000)
            {
                throw new RecursionException("Too many differences");
            }

            if (x + 1 < width && usedPoints.TryGetValue($"{x + 1},{y}", out bool value) && !value)
            {
                if (rect.X + rect.Width <= x + 1)
                {
                    rect.Width = x + 2 - rect.X;
                }
                var p1 = new Point(x + 1, y);
                GetRectangle(ref rect, counter, p1, usedPoints, width, height);
            }

            if (x - 1 >= 0 && usedPoints.TryGetValue($"{x - 1},{y}", out value) && !value)
            {
                if (rect.X > x - 1)
                {
                    rect.Width = rect.Width + rect.X - (x - 1);
                    rect.X = x - 1;
                }
                var p2 = new Point(x - 1, y);
                GetRectangle(ref rect, counter, p2, usedPoints, width, height);
            }

            if (y + 1 < height && usedPoints.TryGetValue($"{x},{y + 1}", out value) && !value)
            {
                if (rect.Y + rect.Height <= y + 1)
                {
                    rect.Height = y + 2 - rect.Y;
                }
                var p3 = new Point(x, y + 1);
                GetRectangle(ref rect, counter, p3, usedPoints, width, height);
            }

            if (y - 1 >= 0 && usedPoints.TryGetValue($"{x},{y - 1}", out value) && !value)
            {
                if (rect.Y > y - 1)
                {
                    rect.Height = rect.Height + rect.Y - (y - 1);
                    rect.Y = y - 1;
                }
                var p4 = new Point(x, y - 1);
                GetRectangle(ref rect, counter, p4, usedPoints, width, height);
            }
        }
    }
}
