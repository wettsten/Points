using System.Collections;
using System.Collections.Generic;

namespace Points.Scheduler.Processors
{
    public interface IJobProcessor
    {
        void ProcessJobs();
        void ScheduleStartJob(string userId);
        void ScheduleEndJob(string userId);
    }
}