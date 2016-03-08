using System.ComponentModel.DataAnnotations;

namespace Points.Model
{
    public class Frequency
    {
        [Required]
        public ModelBase Type { get; set; }
        [Required]
        [Range(1,int.MaxValue)]
        public int? Value { get; set; }
        [Required]
        public ModelBase Unit { get; set; }
    }
}
