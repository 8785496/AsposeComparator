using AsposeComparator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsposeComparator.Interfaces
{
    public interface ICompareService
    {
        CompareResponse CompareImages(string fileName1, string fileName2, int tolerance, int maxDifferences);
        Task<CompareResponse> CompareImagesAsync(string fileName1, string fileName2, int tolerance, int maxDifferences);
        void SetColorComparator(IColorComparator colorComparator);
    }
}
