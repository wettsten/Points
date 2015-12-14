﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Points.Data
{
    public class ActiveTask : PlanningTask
    {
        public int TimesCompleted { get; set; }
        public bool IsCompleted => TimesCompleted >= Frequency.Value;
    }
}
