using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Points.Data.View
{
    public class Duration
    {
        public DurationType Type { get; set; }
        public int? Value { get; set; }
        public DurationUnit Unit { get; set; }
    }
}
