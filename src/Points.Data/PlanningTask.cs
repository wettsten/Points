using System.ComponentModel.DataAnnotations;

namespace Points.Data
{
    public class PlanningTask : DataBase
    {
        public string TaskId { get; set; }
        public Duration Duration { get; set; }
        public Frequency Frequency { get; set; }

        public decimal BonusPointValue
        {
            get
            {
                decimal bonus = 0M;
                if (Frequency.Value != null)
                {
                    bonus = 1/(decimal) Frequency.Value.Value;
                }
                return bonus;
            }
        }

        public override void Copy(DataBase obj)
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
