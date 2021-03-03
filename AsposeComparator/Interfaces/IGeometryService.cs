using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace AsposeComparator.Interfaces
{
    public interface IGeometryService
    {
        List<Rectangle> GetRectangles(int width, int height, List<Point> points, int maxResults);
    }
}
