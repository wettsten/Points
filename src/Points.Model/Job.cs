using System;

namespace Points.Model
{
    public class Job : ViewObject
    {
        public DateTime Trigger { get; set; }
        public string Processor { get; set; }
    }
}