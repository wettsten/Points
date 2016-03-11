using System;
using System.Linq;
using log4net;
using Points.Data;
using Points.DataAccess.Readers;
using Points.DataAccess.Writers;
using Points.Scheduler.Jobs;

namespace Points.Scheduler.Processors
{
    public class JobManager : IJobManager
    {
        private readonly ISingleSessionDataReader _dataReader;
        private readonly ISingleSessionDataWriter _dataWriter;
        private readonly ILog _logger;

        public JobManager(ISingleSessionDataReader dataReader, ISingleSessionDataWriter dataWriter, ILog logger)
        {
            _dataReader = dataReader;
            _dataWriter = dataWriter;
            _logger = logger;
        }

        public void ScheduleStartJob(string userId)
        {
            _logger.InfoFormat("Scheduling start job for {0}", userId);
            var user = _dataReader.Get<User>(userId);
            var startJob = _dataReader.GetAll<Job>()
                .Where(i => i.UserId.Equals(user.Id, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault(i => i.Processor.Equals(typeof (StartWeekJob).Name, StringComparison.InvariantCultureIgnoreCase));
            if (startJob == null)
            {
                _logger.Debug("No existing start job found. Creating new one.");
                startJob = new Job
                {
                    Id = string.Empty,
                    Name = Guid.NewGuid().ToString("N"),
                    Processor = typeof (StartWeekJob).Name,
                    UserId = user.Id,
                    Trigger = FindNextOccurrence(DateTime.UtcNow, user.WeekStartDay, user.WeekStartHour)
                };
                _dataWriter.Add(startJob);
                _logger.Debug("Start job created successfully");
            }
            else
            {
                _logger.Debug("Existing start job found.");
                var endJob = _dataReader.GetAll<Job>()
                    .Where(i => i.UserId.Equals(user.Id, StringComparison.InvariantCultureIgnoreCase))
                    .FirstOrDefault(i => i.Processor.Equals(typeof(EndWeekJob).Name, StringComparison.InvariantCultureIgnoreCase));
                if (endJob == null)
                {
                    _logger.Debug("No existing end job found. Scheduling start job for next occurrence from now.");
                    startJob.Trigger = FindNextOccurrence(DateTime.UtcNow, user.WeekStartDay, user.WeekStartHour);
                }
                else
                {
                    _logger.Debug("Existing end job found. Scheduling start job for next occurrence after current week ends.");
                    startJob.Trigger = FindNextOccurrence(endJob.Trigger, user.WeekStartDay, user.WeekStartHour);
                }
                _dataWriter.Edit(startJob);
                _logger.Debug("Start job edited successfully");
            }
        }

        public void ScheduleEndJob(string userId)
        {
            _logger.InfoFormat("Scheduling end job for {0}", userId);
            var startJob = _dataReader.GetAll<Job>()
                .Where(i => i.UserId.Equals(userId, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault(i => i.Processor.Equals(typeof(StartWeekJob).Name, StringComparison.InvariantCultureIgnoreCase));
            if (startJob != null)
            {
                var endJob = new Job
                {
                    Id = string.Empty,
                    Name = Guid.NewGuid().ToString("N"),
                    Processor = typeof(EndWeekJob).Name,
                    UserId = userId,
                    Trigger = startJob.Trigger.AddDays(7)
                };
                _dataWriter.Add(endJob);
            }
            else
            {
                _logger.Warn("No existing start job found");
            }
        }

        internal DateTime FindNextOccurrence(DateTime start, DayOfWeek day, int hour)
        {
            int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            if (daysToAdd == 0 && start.Hour > hour)
            {
                daysToAdd += 7;
            }
            start = start.AddDays(daysToAdd);
            return new DateTime(start.Year, start.Month, start.Day, hour, 0, 0);
        }
    }
}