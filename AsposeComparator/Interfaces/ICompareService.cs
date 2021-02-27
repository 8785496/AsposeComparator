using AsposeComparator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsposeComparator.Interfaces
{
    public interface ICompareService
    {
        CompareResponse CompareImages(string fileName1, string fileName2, int tolerance = 0, int maxDifferences = 0);
    }
}
