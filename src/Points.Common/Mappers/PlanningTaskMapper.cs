using Points.Data.EnumExtensions;
using Points.Data.Raven;
using Points.Data.View;
using Points.DataAccess;
using Points.DataAccess.Readers;
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
            var viewTask = new ViewPlanningTask();
            viewTask.Copy(_baseMapper.Map(obj));
            var task = _dataReader.Get<RavenTask>(obj.TaskId);
            viewTask.Task = _taskMapper.Map(task);
            viewTask.Duration = new Data.View.Duration
            {
                Type = new Data.View.DurationType
                {
                    Id = obj.Duration.Type.ToString(),
                    Name = obj.Duration.Type.Spacify()
                },
                Value = obj.Duration.Value,
                Unit = new Data.View.DurationUnit
                {
                    Id = obj.Duration.Unit.ToString(),
                    Name = obj.Duration.Unit.Spacify()
                }
            };
            viewTask.Frequency = new Data.View.Frequency
            {
                Type = new Data.View.FrequencyType
                {
                    Id = obj.Frequency.Type.ToString(),
                    Name = obj.Frequency.Type.Spacify()
                },
                Value = obj.Frequency.Value,
                Unit = new Data.View.FrequencyUnit
                {
                    Id = obj.Frequency.Unit.ToString(),
                    Name = obj.Frequency.Unit.Spacify()
                }
            };
            return viewTask;
        }
    }
}