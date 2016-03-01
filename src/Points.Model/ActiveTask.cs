using System;
using System.ComponentModel.DataAnnotations;

namespace Points.Model
{
    public class ActiveTask : PlanningTask
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int TimesCompleted { get; set; }
        public bool IsCompleted { get; set; }
        public decimal BonusPoints { get; set; }
        public DateTime DateStarted { get; set; }
    }
}
