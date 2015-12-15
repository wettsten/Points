using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Points.Data
{
    public class PlanningTask : Task
    {
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
                Duration = task.Duration;
                Frequency = task.Frequency;
            }
        }
    }
}
