using AsposeComparator.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace AsposeComparator.Models
{
    public class RgbColorComparator : IColorComparator
    {
        private readonly double _max;
        private readonly double _min;
        private readonly double _eps;

        public RgbColorComparator()
        {
            _max = Math.Sqrt(255 * 255 + 255 * 255 + 255 * 255);
            _min = 0.0;
            _eps = 0.001;
        }

        public double GetDistance(Color color1, Color color2)
        {
            return Math.Sqrt(Math.Pow(color1.R - color2.R, 2) + Math.Pow(color1.G - color2.G, 2) + Math.Pow(color1.B - color2.B, 2));
        }

        public bool IsEqual(Color color1, Color color2, int tolerance)
        {
            if (tolerance == 0)
            {
                return color1.R == color2.R && color1.G == color2.G && color1.B == color2.B;
            }
            var allowableError = GetErrorValue(tolerance);
            var error = Math.Sqrt(Math.Pow(color1.R - color2.R, 2) + Math.Pow(color1.G - color2.G, 2) + Math.Pow(color1.B - color2.B, 2));
            return error - allowableError < _eps;
        }

        private double GetErrorValue(int tolerance)
        {
            return tolerance * (_max - _min) / 100.0;
        }
    }
}
