﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Points.Data
{
    public class PlanningTask : RavenObject
    {
        public string TaskId { get; set; }
        [Required]
        public Duration Duration { get; set; }
        [Required]
        public Frequency Frequency { get; set; }

        public override void Copy(RavenObject obj)
        {
            base.Copy(obj);
            var task = obj as PlanningTask;
            if (task != null)
            {
                TaskId = task.TaskId;
                Duration = task.Duration;
                Frequency = task.Frequency;
            }
        }
    }
}
