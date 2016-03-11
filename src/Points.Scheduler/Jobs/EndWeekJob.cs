using System;
using System.Linq;
using AutoMapper;
using Points.Data;
using Points.DataAccess.Readers;
using Points.DataAccess.Writers;

namespace Points.Scheduler.Jobs
{
    public class EndWeekJob : IJob
    {
        private readonly ISingleSessionDataReader _dataReader;
        private readonly ISingleSessionDataWriter _dataWriter;
        private readonly IMapper _mapper;

        public EndWeekJob(ISingleSessionDataReader dataReader, ISingleSessionDataWriter dataWriter, IMapper mapper)
        {
            _dataReader = dataReader;
            _dataWriter = dataWriter;
            _mapper = mapper;
        }

        public void Process(Job context)
        {
            var tasks = _dataReader
                .GetAll<ActiveTask>()
                .Where(i => i.UserId.Equals(context.UserId, StringComparison.InvariantCultureIgnoreCase));
            foreach (var task in tasks)
            {
                var archiveTask = new ArchivedTask();
                _mapper.Map(task, archiveTask);
                archiveTask.Id = string.Empty;
                archiveTask.DateEnded = DateTime.UtcNow;
                _dataWriter.Add(archiveTask);
                _dataWriter.Delete<ActiveTask>(task.Id);
            }
        }
    }
}