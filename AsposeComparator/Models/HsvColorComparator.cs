using AsposeComparator.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace AsposeComparator.Models
{
    public class HsvColorComparator : IColorComparator
    {
        private readonly double _max;
        private readonly double _min;
        private readonly double _eps;

        class HsvColor
        {
            public double H { get; set; }
            public double S { get; set; }
            public double V { get; set; }

            public HsvColor(Color color)
            {
                var r = color.R;
                var g = color.G;
                var b = color.B;
                var max = Math.Max(r, Math.Max(g, b));
                var min = Math.Min(r, Math.Min(g, b));

                if (max == min)
                {
                    H = 0.0;
                } else if (max == r && g >= b)
                {
                    H = 60.0 * (g - b) / (max - min);
                } 
                else if (max == r && g < b)
                {
                    H = 60.0 * (g - b) / (max - min) + 360.0;
                } 
                else if (max == g)
                {
                    H = 60.0 * (b - r) / (max - min) + 120.0;
                }
                else if (max == b)
                {
                    H = 60.0 * (r - g) / (max - min) + 240.0;
                }

                S = max == 0 ? 0.0 : 1 - (double)min / (double)max;
                V = max;
            }
        }

        public HsvColorComparator()
        {
            _max = Math.Sqrt(3);
            _min = 0.0;
            _eps = 0.0000001;
        }
        
        public bool IsEqual(Color color1, Color color2, int tolerance)
        {
            if (tolerance == 0)
            {
                return color1.R == color2.R && color1.G == color2.G && color1.B == color2.B;
            }
            var hsv1 = new HsvColor(color1);
            var hsv2 = new HsvColor(color2);
            var allowableError = GetErrorValue(tolerance);
            var error = Math.Sqrt(Math.Pow((hsv1.H - hsv2.H) / 360.0, 2) + Math.Pow(hsv1.S - hsv2.S, 2) + Math.Pow((hsv1.V - hsv2.V) / 255.0, 2));
            return error - allowableError < _eps;
        }

        private double GetErrorValue(int tolerance)
        {
            return tolerance * (_max - _min) / 100.0;
        }
    }
}
