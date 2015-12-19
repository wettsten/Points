using System.Collections;
using System.Collections.Generic;

namespace Points.Scheduler.Processors
{
    public interface IJobProcessor
    {
        void ProcessJobs();
        void ScheduleJob<T>(string userId) where T : IJob;
    }
}