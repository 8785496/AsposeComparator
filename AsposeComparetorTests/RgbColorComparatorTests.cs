using NUnit.Framework;
using AsposeComparator.Models;
using System.Drawing;

namespace AsposeComparatorTests
{
    public class RgbColorComparatorTests
    {
        private RgbColorComparator _comparator;

        [SetUp]
        public void Setup()
        {
            _comparator = new RgbColorComparator();
        }

        [Test]
        public void CompareRgbColorsRedAndGreenReturnFalse()
        {
            var color1 = Color.FromArgb(255, 0, 0);
            var color2 = Color.FromArgb(0, 255, 0);
            Assert.IsFalse(_comparator.IsEqual(color1, color2, 25));
        }

        [Test]
        public void CompareRgbColorsRedAndPinkReturnTrue()
        {
            var color1 = Color.FromArgb(255, 0, 0);
            var color2 = Color.FromArgb(255, 128, 192);
            Assert.IsTrue(_comparator.IsEqual(color1, color2, 75));
        }

        [Test]
        public void CompareRgbColorsBlackAndWhiteReturnTrue()
        {
            var color1 = Color.FromArgb(0, 0, 0);
            var color2 = Color.FromArgb(255, 255, 255);
            Assert.IsTrue(_comparator.IsEqual(color1, color2, 100));
        }
    }
}