using System;

namespace Points.Data
{
    public class ActiveTask : DataBase
    {
        public string TaskName { get; set; }
        public string CategoryName { get; set; }
        public int TimesCompleted { get; set; }
        public DateTime DateStarted { get; set; }
        public Duration Duration { get; set; }
        public Frequency Frequency { get; set; }
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
                        ? (Frequency.Value.Value - TimesCompleted) / (decimal)Frequency.Value.Value
                        : (TimesCompleted - Frequency.Value.Value) / (decimal)Frequency.Value.Value;
                }
                return bonus;
            }
        }
        public decimal BonusPointValue
        {
            get
            {
                decimal bonus = 0M;
                if (Frequency.Value != null)
                {
                    bonus = 1 / (decimal)Frequency.Value.Value;
                }
                return bonus;
            }
        }

        public override void Copy(DataBase obj)
        {
            base.Copy(obj);
            var task = obj as ActiveTask;
            if (task != null)
            {
                TaskName = task.TaskName;
                CategoryName = task.CategoryName;
                Duration = task.Duration;
                Frequency = task.Frequency;
                TimesCompleted = task.TimesCompleted;
                DateStarted = task.DateStarted;
            }
        }
    }
}
