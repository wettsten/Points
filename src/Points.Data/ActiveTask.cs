using System;
using System.ComponentModel.DataAnnotations;

namespace Points.Data
{
    public class ActiveTask : PlanningTask
    {
        [Range(0,int.MaxValue)]
        public int TimesCompleted { get; set; }

        public bool IsCompleted
        {
            get
            {
                bool completed = false;
                if (Frequency.Value != null)
                {
                    completed = Frequency.Type == FrequencyType.AtMost
                        ? TimesCompleted <= Frequency.Value.Value
                        : TimesCompleted >= Frequency.Value.Value;
                }
                return completed;
            }
        }

        public decimal BonusPoints
        {
            get
            {
                decimal bonus = 0M;
                if (Frequency.Value != null && IsCompleted)
                {
                    bonus = Frequency.Type == FrequencyType.AtMost
                        ? (Frequency.Value.Value - TimesCompleted) / Frequency.Value.Value
                        : (TimesCompleted - Frequency.Value.Value) / Frequency.Value.Value;
                }
                return bonus;
            }
        }

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
