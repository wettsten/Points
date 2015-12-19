using System;

namespace Points.Scheduler.Factories
{
    public interface IJobFactory
    {
        IJob GetJobProcessor(string type);
    }
}