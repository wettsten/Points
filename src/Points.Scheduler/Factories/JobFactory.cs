using System;
using System.Collections.Generic;
using System.Linq;

namespace Points.Scheduler.Factories
{
    public class JobFactory : IJobFactory
    {
        private readonly IDictionary<string, IJob> _iJobs;

        public JobFactory(IList<IJob> iJobs)
        {
            _iJobs = iJobs.ToDictionary(i => i.GetType().ToString(), j => j);
        }

        public IJob GetJobProcessor(string type)
        {
            if (_iJobs.ContainsKey(type))
            {
                return _iJobs[type];
            }
            return null;
        }
    }
}