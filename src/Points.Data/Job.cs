using System;

namespace Points.Data
{
    public class Job : DataBase
    {
        public DateTime Trigger { get; set; }
        public string Processor { get; set; }

        public override void Copy(DataBase obj)
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