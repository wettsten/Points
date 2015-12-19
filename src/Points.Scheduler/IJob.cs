using System.Collections.Generic;
using Points.Data.Raven;

namespace Points.Scheduler
{
    public interface IJob
    {
        void Process(Job context);
    }
}