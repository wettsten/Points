using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Points.Data
{
    public class Frequency : IValidatableObject
    {
        [Required]
        public FrequencyType Type { get; set; }
        public int? Value { get; set; }
        public FrequencyUnit? Unit { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Type.Equals(FrequencyType.Once))
            {
                if (!Value.HasValue)
                {
                    yield return new ValidationResult("Frequency Value must be provided.");
                }
                if (!Unit.HasValue)
                {
                    yield return new ValidationResult("Frequency Unit must be provided.");
                }
            }
        }
    }
}
