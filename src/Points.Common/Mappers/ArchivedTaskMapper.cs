using RavenArchivedTask = Points.Data.Raven.ArchivedTask;
using ViewArchivedTask = Points.Data.View.ArchivedTask;
using RavenActiveTask = Points.Data.Raven.ActiveTask;
using ViewActiveTask = Points.Data.View.ActiveTask;

namespace Points.Common.Mappers
{
    public class ArchivedTaskMapper : IObjectMapper<RavenArchivedTask, ViewArchivedTask>
    {
        private readonly IObjectMapper<RavenActiveTask, ViewActiveTask> _taskMapper;

        public ArchivedTaskMapper(IObjectMapper<RavenActiveTask, ViewActiveTask> taskMapper)
        {
            _taskMapper = taskMapper;
        }

        public ViewArchivedTask Map(RavenArchivedTask obj)
        {
            var viewTask = new ViewArchivedTask();
            viewTask.Copy(_taskMapper.Map(obj));
            viewTask.TimesCompleted = obj.TimesCompleted;
            return viewTask;
        }
    }
}