using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace AsposeComparator.Models
{
    public class CompareResponse
    {
        public string FileName1 { get; set; }
        public string FileName2 { get; set; }
        public string ResultFileName { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
