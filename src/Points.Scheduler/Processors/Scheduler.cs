
using System;
using System.Threading;

namespace Points.Scheduler.Processors
{
    public class Scheduler : IScheduler
    {
        private readonly IJobProcessor _jobProcessor;
        private readonly Timer _hourTimer;

        public Scheduler(IJobProcessor jobProcessor)
        {
            _jobProcessor = jobProcessor;
            _hourTimer = new Timer(HourTick);
        }

        public void Start()
        {
            var now = DateTime.UtcNow.AddHours(1);
            var ts = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0) - DateTime.UtcNow;
            _hourTimer.Change(ts, TimeSpan.FromHours(1));
        }

        private void HourTick(object t)
        {
            _jobProcessor.ProcessJobs();
        }
    }
}