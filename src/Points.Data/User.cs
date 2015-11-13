using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspNet.Identity.RavenDB.Entities;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Raven.Imports.Newtonsoft.Json;

namespace Points.Data
{
    public class User : RavenObject
    {
        //add user options here
        public string Email { get; set; }
        public bool DefaultTasksToPrivate { get; set; }
        public DayOfWeek WeekStartDay { get; set; }
        public int WeekStartHour { get; set; }
        public bool NotifyWeekStarting { get; set; }
        public bool NotifyWeekEnding { get; set; }
    }
}
