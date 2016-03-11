using System;
using System.ComponentModel.DataAnnotations;

namespace Points.Model
{
    public class ActiveTask : ModelBase
    {
        public string TaskName { get; set; }
        public string CategoryName { get; set; }
        [Required]
        public Duration Duration { get; set; }
        [Required]
        public Frequency Frequency { get; set; }
        public decimal BonusPointValue { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int TimesCompleted { get; set; }
        public bool IsCompleted { get; set; }
        public decimal BonusPoints { get; set; }
        public DateTime DateStarted { get; set; }
    }
}
