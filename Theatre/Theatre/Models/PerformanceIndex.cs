using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Theatre.Models
{
    public class PerformanceIndex
    {
        public IEnumerable<Performance> Performances { get; set; }
        public Performance Performance { get; set; }
        public int CurrentRole { get; set; }
    }
}
