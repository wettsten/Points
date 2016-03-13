using System;
using System.Linq;
using AutoMapper;
using log4net;
using Points.Data;
using Points.DataAccess.Readers;
using Points.DataAccess.Writers;
using Points.Scheduler.Processors;

namespace Points.Scheduler.Jobs
{
    public class StartWeekJob : IJob
    {
        private readonly ISingleSessionDataReader _dataReader;
        private readonly ISingleSessionDataWriter _dataWriter;
        private readonly IJobManager _jobManager;
        private readonly IMapper _mapper;
        private readonly ILog _logger = LogManager.GetLogger("Scheduler");

        public StartWeekJob(ISingleSessionDataReader dataReader, ISingleSessionDataWriter dataWriter, IJobManager jobManager, IMapper mapper)
        {
            _dataReader = dataReader;
            _dataWriter = dataWriter;
            _jobManager = jobManager;
            _mapper = mapper;
        }

        public void Process(Job context)
        {
            _logger.InfoFormat("Processing start week job for user {0}", context.UserId);
            var tasks = _dataReader
                .GetAll<PlanningTask>()
                .Where(i => i.UserId.Equals(context.UserId, StringComparison.InvariantCultureIgnoreCase));
            _logger.DebugFormat("Found {0} tasks to activate", tasks.Count());
            foreach (var task in tasks)
            {
                _logger.DebugFormat("Activating task {0}", task.Name);
                var activeTask = new ActiveTask();
                _mapper.Map(task, activeTask);
                activeTask.Id = string.Empty;
                activeTask.DateStarted = DateTime.UtcNow;
                _dataWriter.Add(activeTask);
            }
            if (tasks.Any())
            {
                _jobManager.ScheduleEndJob(context.UserId);

                _logger.Debug("Updating Active Target Points");
                var user = _dataReader.Get<User>(context.UserId);
                user.ActiveTargetPoints = user.TargetPoints;
                _dataWriter.Edit(user);
            }
            _jobManager.ScheduleStartJob(context.UserId);
            _logger.InfoFormat("Finished processing start week job for user {0}", context.UserId);
        }
    }
}