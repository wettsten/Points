using System;

namespace Points.Data
{
    public class Job : RavenObject
    {
        public DateTime Trigger { get; set; }
        public string Processor { get; set; }

        public override void Copy(RavenObject obj)
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