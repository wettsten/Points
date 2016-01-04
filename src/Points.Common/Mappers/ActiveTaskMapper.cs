using RavenActiveTask = Points.Data.Raven.ActiveTask;
using ViewActiveTask = Points.Data.View.ActiveTask;
using RavenPlanningTask = Points.Data.Raven.PlanningTask;
using ViewPlanningTask = Points.Data.View.PlanningTask;

namespace Points.Common.Mappers
{
    public class ActiveTaskMapper : IObjectMapper<RavenActiveTask, ViewActiveTask>
    {
        private readonly IObjectMapper<RavenPlanningTask, ViewPlanningTask> _taskMapper;

        public ActiveTaskMapper(IObjectMapper<RavenPlanningTask, ViewPlanningTask> taskMapper)
        {
            _taskMapper = taskMapper;
        }

        public ViewActiveTask Map(RavenActiveTask obj)
        {
            var viewTask = new ViewActiveTask();
            viewTask.Copy(_taskMapper.Map(obj));
            viewTask.TimesCompleted = obj.TimesCompleted;
            viewTask.DateStarted = obj.DateStarted;
            return viewTask;
        }
    }
}