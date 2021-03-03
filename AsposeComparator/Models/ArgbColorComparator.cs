using AsposeComparator.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace AsposeComparator.Models
{
    public class ArgbColorComparator : IColorComparator
    {
        private readonly double _max;
        private readonly double _min;
        private readonly double _eps;

        public ArgbColorComparator()
        {
            _max = Math.Sqrt(255 * 255 + 255 * 255 + 255 * 255);
            _min = 0.0;
            _eps = 0.001;
        }

        public double GetDistance(Color color1, Color color2)
        {
            var rgb1 = ConvertAgrbToRgb(color1);
            var rgb2 = ConvertAgrbToRgb(color2);
            return Math.Sqrt(Math.Pow(rgb1.R - rgb2.R, 2) + Math.Pow(rgb1.G - rgb2.G, 2) + Math.Pow(rgb1.B - rgb2.B, 2));
        }

        public bool IsEqual(Color color1, Color color2, int tolerance)
        {
            var rgb1 = ConvertAgrbToRgb(color1);
            var rgb2 = ConvertAgrbToRgb(color2);

            if (tolerance == 0)
            {
                return rgb1.R == rgb2.R && rgb1.G == rgb2.G && rgb1.B == rgb2.B;
            }
            var allowableError = GetErrorValue(tolerance);
            var error = Math.Sqrt(Math.Pow(rgb1.R - rgb2.R, 2) + Math.Pow(rgb1.G - rgb2.G, 2) + Math.Pow(rgb1.B - rgb2.B, 2));
            return error - allowableError < _eps;
        }

        private Color ConvertAgrbToRgb(Color color)
        {
            if (color.A == 255)
            {
                return color;
            }
            var r = (byte)Math.Round((255 - color.A) + color.A / 255.0 * color.R, 0);
            var g = (byte)Math.Round((255 - color.A) + color.A / 255.0 * color.G, 0);
            var b = (byte)Math.Round((255 - color.A) + color.A / 255.0 * color.B, 0);

            return Color.FromArgb(r, g, b);
        }

        private double GetErrorValue(int tolerance)
        {
            return tolerance * (_max - _min) / 100.0;
        }
    }
}
