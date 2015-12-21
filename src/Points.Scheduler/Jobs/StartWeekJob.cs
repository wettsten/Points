using System;
using System.Collections.Generic;
using System.Linq;
using Points.Data.Raven;
using Points.DataAccess;
using Points.Scheduler.Processors;

namespace Points.Scheduler.Jobs
{
    public class StartWeekJob : IJob
    {
        private readonly IDataReader _dataReader;
        private readonly IDataWriter _dataWriter;
        private readonly IJobManager _jobManager;

        public StartWeekJob(IDataReader dataReader, IDataWriter dataWriter, IJobManager jobManager)
        {
            _dataReader = dataReader;
            _dataWriter = dataWriter;
            _jobManager = jobManager;
        }

        public void Process(Job context)
        {
            var tasks = _dataReader
                .GetAll<PlanningTask>()
                .Where(i => i.UserId.Equals(context.UserId, StringComparison.InvariantCultureIgnoreCase))
                .Where(i => !i.IsDeleted);
            foreach (var task in tasks)
            {
                var activeTask = new ActiveTask
                {
                    Name = _dataReader.Get<Task>(task.TaskId)?.Name
                };
                activeTask.Copy(task);
                activeTask.Id = string.Empty;
                _dataWriter.Add(activeTask);
            }
            _jobManager.ScheduleEndJob(context.UserId);
            _jobManager.ScheduleStartJob(context.UserId);
        }
    }
}