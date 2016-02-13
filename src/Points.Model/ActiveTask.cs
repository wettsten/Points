
using System;

namespace Points.Model
{
    public class ActiveTask : PlanningTask
    {
        public int TimesCompleted { get; set; }
        public bool IsCompleted => TimesCompleted >= Frequency.Value;
        public DateTime DateStarted { get; set; }

        public override void Copy(ViewObject obj)
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
