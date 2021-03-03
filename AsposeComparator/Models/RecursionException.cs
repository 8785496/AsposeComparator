using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsposeComparator.Models
{
    public class RecursionException : Exception
    {
        public RecursionException(string message) : base(message) { }
    }
}
