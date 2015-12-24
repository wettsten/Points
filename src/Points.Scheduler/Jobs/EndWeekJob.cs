using System;
using System.Collections.Generic;
using System.Linq;
using Points.Data.Raven;
using Points.DataAccess;

namespace Points.Scheduler.Jobs
{
    public class EndWeekJob : IJob
    {
        private readonly IDataReader _dataReader;
        private readonly IDataWriter _dataWriter;

        public EndWeekJob(IDataReader dataReader, IDataWriter dataWriter)
        {
            _dataReader = dataReader;
            _dataWriter = dataWriter;
        }

        public void Process(Job context)
        {
            var tasks = _dataReader
                .GetAll<ActiveTask>()
                .Where(i => i.UserId.Equals(context.UserId, StringComparison.InvariantCultureIgnoreCase));
            foreach (var task in tasks)
            {
                var archiveTask = new ArchivedTask
                {
                    Id = string.Empty,
                    Name = task.Name
                };
                archiveTask.Copy(task);
                _dataWriter.Add(archiveTask);
            }
        }
    }
}