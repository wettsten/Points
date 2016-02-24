using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Points.Model
{
    public class User : ViewObject
    {
        public string Email { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public DayOfWeek WeekStartDay { get; set; }
        public SimpleInt WeekStartHour { get; set; }
        public SimpleInt NotifyWeekStarting { get; set; }
        public SimpleInt NotifyWeekEnding { get; set; }
        public DateTime? PlanningEndTime { get; set; }
        public DateTime? ActiveStartTime { get; set; }
        public int TargetPoints { get; set; }
        public bool EnableAdvancedFeatures { get; set; }
        public int CategoryBonus { get; set; }
        public int TaskMultiplier { get; set; }
        public int BonusPointMultiplier { get; set; }
        public decimal DurationBonusPointsPerHour { get; set; }

        public override void Copy(ViewObject obj)
        {
            base.Copy(obj);
            var user = obj as User;
            if (user != null)
            {
                Email = user.Email;
                WeekStartDay = user.WeekStartDay;
                WeekStartHour = user.WeekStartHour;
                NotifyWeekStarting = user.NotifyWeekStarting;
                NotifyWeekEnding = user.NotifyWeekEnding;
                PlanningEndTime = user.PlanningEndTime;
                ActiveStartTime = user.ActiveStartTime;
                TargetPoints = user.TargetPoints;
                EnableAdvancedFeatures = user.EnableAdvancedFeatures;
                CategoryBonus = user.CategoryBonus;
                TaskMultiplier = user.TaskMultiplier;
                BonusPointMultiplier = user.BonusPointMultiplier;
                DurationBonusPointsPerHour = user.DurationBonusPointsPerHour;
            }
        }
    }
}
