using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Points.Data
{
    public class Duration
    {
        public DurationType Type { get; set; }
        public int Value { get; set; }
        public DurationUnit Unit { get; set; }
    }
}
