using System.ComponentModel.DataAnnotations;

namespace Points.Model
{
    public class PlanningTask : ModelBase
    {
        [Required]
        public Task Task { get; set; }
        [Required]
        public Duration Duration { get; set; }
        [Required]
        public Frequency Frequency { get; set; }
        public decimal BonusPointValue { get; set; }
    }
}
