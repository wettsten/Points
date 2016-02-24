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
        public int WeekStartHour { get; set; }
        public int NotifyWeekStarting { get; set; }
        public int NotifyWeekEnding { get; set; }
        public DateTime? PlanningEndTime { get; set; }
        public DateTime? ActiveStartTime { get; set; }

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
            }
        }
    }
}
