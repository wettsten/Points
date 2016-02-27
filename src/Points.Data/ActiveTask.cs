﻿using System;

namespace Points.Data
{
    public class ActiveTask : PlanningTask
    {
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
                        ? (Frequency.Value.Value - TimesCompleted) / (decimal)Frequency.Value.Value
                        : (TimesCompleted - Frequency.Value.Value) / (decimal)Frequency.Value.Value;
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
