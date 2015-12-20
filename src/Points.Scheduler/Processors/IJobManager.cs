using System.Collections;
using System.Collections.Generic;

namespace Points.Scheduler.Processors
{
    public interface IJobManager
    {
        void ScheduleStartJob(string userId);
        void ScheduleEndJob(string userId);
    }
}