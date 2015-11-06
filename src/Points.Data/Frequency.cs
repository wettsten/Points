using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Points.Data
{
    public class Frequency
    {
        public FrequencyLimit Limit { get; set; }
        public int Value { get; set; }
        public FrequencyUnit Unit { get; set; }
    }
}
