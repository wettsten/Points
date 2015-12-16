using Points.Data.Raven;
using Points.Data.View;
using Points.DataAccess;
using RavenTask = Points.Data.Raven.Task;
using ViewTask = Points.Data.View.Task;
using RavenPlanningTask = Points.Data.Raven.PlanningTask;
using ViewPlanningTask = Points.Data.View.PlanningTask;

namespace Points.Common.Mappers
{
    public class PlanningTaskMapper : IObjectMapper<RavenPlanningTask, ViewPlanningTask>
    {
        private readonly IDataReader _dataReader;
        private readonly IObjectMapper<RavenTask, ViewTask> _taskMapper;
        private readonly IObjectMapper<RavenObject, ViewObject> _baseMapper;

        public PlanningTaskMapper(IDataReader dataReader, IObjectMapper<RavenObject, ViewObject> baseMapper, IObjectMapper<RavenTask, ViewTask> taskMapper)
        {
            _dataReader = dataReader;
            _baseMapper = baseMapper;
            _taskMapper = taskMapper;
        }

        public ViewPlanningTask Map(RavenPlanningTask obj)
        {
            var viewTask = (ViewPlanningTask)_baseMapper.Map(obj);
            var task = _dataReader.Get<RavenTask>(obj.TaskId);
            viewTask.Task = _taskMapper.Map(task);
            viewTask.Duration = obj.Duration;
            viewTask.Frequency = obj.Frequency;
            return viewTask;
        }
    }
}