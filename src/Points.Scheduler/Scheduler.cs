using Quartz.Impl;

namespace Points.Scheduler
{
    public class Scheduler : IScheduler
    {
        public void Start()
        {
            var scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();
        }
    }
}