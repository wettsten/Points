using System;
using System.Collections.Generic;
using System.Linq;
using Points.Data.Raven;
using Points.DataAccess;
using Points.DataAccess.Readers;
using Points.DataAccess.Writers;

namespace Points.Scheduler.Jobs
{
    public class EndWeekJob : IJob
    {
        private readonly ISingleSessionDataReader _dataReader;
        private readonly ISingleSessionDataWriter _dataWriter;

        public EndWeekJob(ISingleSessionDataReader dataReader, ISingleSessionDataWriter dataWriter)
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
                var archiveTask = new ArchivedTask();
                archiveTask.Copy(task);
                archiveTask.Id = string.Empty;
                archiveTask.Name = task.Name;
                archiveTask.DateEnded = DateTime.UtcNow;
                _dataWriter.Add(archiveTask);
            }
        }
    }
}