using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Points.Data.View
{
    public class Frequency
    {
        public FrequencyType Type { get; set; }
        public int? Value { get; set; }
        public FrequencyUnit Unit { get; set; }
    }
}
