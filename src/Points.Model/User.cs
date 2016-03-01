using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;

namespace Points.Model
{
    public class User : ViewObject
    {
        [Required]
        public string Email { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public DayOfWeek WeekStartDay { get; set; }
        [Required]
        public SimpleInt WeekStartHour { get; set; }
        [Required]
        public SimpleInt NotifyWeekStarting { get; set; }
        [Required]
        public SimpleInt NotifyWeekEnding { get; set; }
        public DateTime? PlanningEndTime { get; set; }
        public DateTime? ActiveStartTime { get; set; }
        [Required]
        public bool WeekSummaryEmail { get; set; }
        [Required]
        public int TargetPoints { get; set; }
        [Required]
        public int ActiveTargetPoints { get; set; }
        [Required]
        public bool EnableAdvancedFeatures { get; set; }
        [Required]
        public int CategoryBonus { get; set; }
        [Required]
        public int TaskMultiplier { get; set; }
        [Required]
        public int BonusPointMultiplier { get; set; }
        [Required]
        public decimal DurationBonusPointsPerHour { get; set; }
    }
}
