using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Points.Data
{
    public class Duration : IValidatableObject
    {
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public DurationType Type { get; set; }
        public int? Value { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public DurationUnit? Unit { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Type.Equals(FrequencyType.Once))
            {
                if (!Value.HasValue)
                {
                    yield return new ValidationResult("Duration Value must be provided.");
                }
                if (!Unit.HasValue)
                {
                    yield return new ValidationResult("Duration Unit must be provided.");
                }
            }
        }
    }
}
