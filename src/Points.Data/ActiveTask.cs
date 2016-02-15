using System;
using System.ComponentModel.DataAnnotations;

namespace Points.Data
{
    public class ActiveTask : PlanningTask
    {
        [Range(0,int.MaxValue)]
        public int TimesCompleted { get; set; }
        public bool IsCompleted => TimesCompleted >= Frequency.Value;
        public DateTime DateStarted { get; set; }

        public override void Copy(RavenObject obj)
        {
            base.Copy(obj);
            var task = obj as ActiveTask;
            if (task != null)
            {
                TimesCompleted = task.TimesCompleted;
                DateStarted = task.DateStarted;
            }
        }
    }
}
