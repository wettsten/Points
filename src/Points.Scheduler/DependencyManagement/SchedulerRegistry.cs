using Points.Scheduler.Processors;
using StructureMap;
using StructureMap.Graph;

namespace Points.Scheduler.DependencyManagement
{
    public class SchedulerRegistry : Registry
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="SchedulerRegistry" /> class from being created.
        /// </summary>
        public SchedulerRegistry()
        {
            Scan(
                scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                    scan.AddAllTypesOf<IJob>();
                });
            ForSingletonOf<IScheduler>().Use<Processors.Scheduler>();
        }
    }
}