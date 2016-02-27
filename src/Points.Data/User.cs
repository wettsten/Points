using System;

namespace Points.Data
{
    public class User : RavenObject
    {
        public string Email { get; set; }
        public DayOfWeek WeekStartDay { get; set; }
        public int WeekStartHour { get; set; }
        public int NotifyWeekStarting { get; set; }
        public int NotifyWeekEnding { get; set; }
        public bool WeekSummaryEmail { get; set; }
        public int TargetPoints { get; set; }
        public int ActiveTargetPoints { get; set; }
        public bool EnableAdvancedFeatures { get; set; }
        public int CategoryBonus { get; set; }
        public int TaskMultiplier { get; set; }
        public int BonusPointMultiplier { get; set; }
        public decimal DurationBonusPointsPerHour { get; set; }

        public override void Copy(RavenObject obj)
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
                TargetPoints = user.TargetPoints;
                ActiveTargetPoints = user.ActiveTargetPoints;
                EnableAdvancedFeatures = user.EnableAdvancedFeatures;
                CategoryBonus = user.CategoryBonus;
                TaskMultiplier = user.TaskMultiplier;
                BonusPointMultiplier = user.BonusPointMultiplier;
                DurationBonusPointsPerHour = user.DurationBonusPointsPerHour;
                WeekSummaryEmail = user.WeekSummaryEmail;
            }
        }
    }
}
