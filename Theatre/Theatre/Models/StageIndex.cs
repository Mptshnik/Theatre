using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Theatre.Models
{
    public class StageIndex
    {
        public IEnumerable<Stage> Stages { get; set; }
        public Stage Stage { get; set; }
        public int CurrentRole { get; set; }
    }
}
