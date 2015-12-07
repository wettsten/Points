﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Points.Data
{
    public class User : RavenObject
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public bool DefaultTasksToPrivate { get; set; }
        [Required]
        public DayOfWeek WeekStartDay { get; set; }
        [Required]
        public int WeekStartHour { get; set; }
        [Required]
        public bool NotifyWeekStarting { get; set; }
        [Required]
        public bool NotifyWeekEnding { get; set; }
    }
}
