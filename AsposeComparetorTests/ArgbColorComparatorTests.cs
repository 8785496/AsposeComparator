using NUnit.Framework;
using AsposeComparator.Models;
using System.Drawing;

namespace AsposeComparatorTests
{
    class ArgbColorComparatorTests
    {
        private ArgbColorComparator _comparator;

        [SetUp]
        public void Setup()
        {
            _comparator = new ArgbColorComparator();
        }

        [Test]
        public void CompareRgbColorsRedAndGreenReturnFalse()
        {
            var color1 = Color.FromArgb(255, 255, 0, 0);
            var color2 = Color.FromArgb(255, 0, 255, 0);
            Assert.IsFalse(_comparator.IsEqual(color1, color2, 25));
        }

        [Test]
        public void CompareRgbColorsRedAndPinkReturnTrue()
        {
            var color1 = Color.FromArgb(255, 255, 0, 0);
            var color2 = Color.FromArgb(200, 255, 0, 0);
            Assert.IsTrue(_comparator.IsEqual(color1, color2, 50));
        }

        [Test]
        public void CompareRgbColorsBlackAndWhiteReturnTrue()
        {
            var color1 = Color.FromArgb(0, 0, 0, 0);
            var color2 = Color.FromArgb(255, 255, 255, 255);
            Assert.IsTrue(_comparator.IsEqual(color1, color2, 0));
        }
    }
}
