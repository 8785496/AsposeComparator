using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace AsposeComparator.Interfaces
{
    public interface IColorComparator
    {
        bool IsEqual(Color color1, Color color2, int tolerance);
        double GetDistance(Color color1, Color color2);
    }
}
