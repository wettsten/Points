﻿using System;

namespace Points.Model
{
    public class Job : ViewObject
    {
        public DateTime Trigger { get; set; }
        public string Processor { get; set; }

        public override void Copy(ViewObject obj)
        {
            base.Copy(obj);
            var job = obj as Job;
            if (job != null)
            {
                Trigger = job.Trigger;
                Processor = job.Processor;
            }
        }
    }
}