using System;
using System.Linq;
using AutoMapper;
using NLog;
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
        private readonly ILogger _logger = LogManager.GetLogger("Scheduler");

        public EndWeekJob(ISingleSessionDataReader dataReader, ISingleSessionDataWriter dataWriter, IMapper mapper)
        {
            _dataReader = dataReader;
            _dataWriter = dataWriter;
            _mapper = mapper;
        }

        public void Process(Job context)
        {
            _logger.Info("Processing end week job for user {0}", context.UserId);
            var tasks = _dataReader
                .GetAll<ActiveTask>()
                .Where(i => i.UserId.Equals(context.UserId, StringComparison.InvariantCultureIgnoreCase));
            _logger.Debug("Found {0} tasks to archive", tasks.Count());
            foreach (var task in tasks)
            {
                _logger.Debug("Archiving task {0}", task.Name);
                var archiveTask = new ArchivedTask();
                _mapper.Map(task, archiveTask);
                archiveTask.Id = string.Empty;
                archiveTask.DateEnded = DateTime.UtcNow;
                _dataWriter.Add(archiveTask);
                _dataWriter.Delete<ActiveTask>(task.Id);
            }
            _logger.Info("Finished processing end week job for user {0}", context.UserId);
        }
    }
}