using System;

namespace Points.Model
{
    public class Job : ModelBase
    {
        public DateTime Trigger { get; set; }
        public string Processor { get; set; }
    }
}