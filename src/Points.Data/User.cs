using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Points.Data
{
    public class User : RavenObject
    {
        public string Email { get; set; }
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public DayOfWeek WeekStartDay { get; set; }
        [Required]
        public int WeekStartHour { get; set; }
        public bool NotifyWeekStarting { get; set; }
        public bool NotifyWeekEnding { get; set; }

        public override void Copy(RavenObject obj)
        {
            base.Copy(obj);
            var user = obj as User;
            if (user != null)
            {
                WeekStartDay = user.WeekStartDay;
                WeekStartHour = user.WeekStartHour;
                NotifyWeekStarting = user.NotifyWeekStarting;
                NotifyWeekEnding = user.NotifyWeekEnding;
            }
        }
    }
}
