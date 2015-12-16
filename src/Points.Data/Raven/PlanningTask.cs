using System.ComponentModel.DataAnnotations;
using Points.Data.Common;

namespace Points.Data.Raven
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
