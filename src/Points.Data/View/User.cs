using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Points.Data.View
{
    public class User : ViewObject
    {
        public string Email { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public DayOfWeek WeekStartDay { get; set; }
        public int WeekStartHour { get; set; }
        public bool NotifyWeekStarting { get; set; }
        public bool NotifyWeekEnding { get; set; }

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
