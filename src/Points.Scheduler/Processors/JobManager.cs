using System;
using System.Linq;
using Points.Data.Raven;
using Points.DataAccess;
using Points.DataAccess.Readers;
using Points.DataAccess.Writers;
using Points.Scheduler.Factories;
using Points.Scheduler.Jobs;

namespace Points.Scheduler.Processors
{
    public class JobManager : IJobManager
    {
        private readonly ISingleSessionDataReader _dataReader;
        private readonly ISingleSessionDataWriter _dataWriter;

        public JobManager(ISingleSessionDataReader dataReader, ISingleSessionDataWriter dataWriter)
        {
            _dataReader = dataReader;
            _dataWriter = dataWriter;
        }

        public void ScheduleStartJob(string userId)
        {
            var user = _dataReader.Get<User>(userId);
            var startJob = _dataReader.GetAll<Job>()
                .Where(i => i.UserId.Equals(user.Id, StringComparison.InvariantCultureIgnoreCase))
                .Where(i => i.Processor.Equals(typeof(StartWeekJob).Name, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault(i => i.Trigger > DateTime.UtcNow);
            if (startJob == null)
            {
                startJob = new Job
                {
                    Id = string.Empty,
                    Name = Guid.NewGuid().ToString("N"),
                    Processor = typeof (StartWeekJob).Name,
                    UserId = user.Id,
                    Trigger = FindNextOccurrence(DateTime.UtcNow, user.WeekStartDay, user.WeekStartHour)
                };
                _dataWriter.Add(startJob);
            }
            else
            {
                var endJob = _dataReader.GetAll<Job>()
                    .Where(i => i.UserId.Equals(user.Id, StringComparison.InvariantCultureIgnoreCase))
                    .FirstOrDefault(i => i.Processor.Equals(typeof(EndWeekJob).Name, StringComparison.InvariantCultureIgnoreCase));
                if (endJob == null)
                {
                    startJob.Trigger = FindNextOccurrence(DateTime.UtcNow, user.WeekStartDay, user.WeekStartHour);
                    _dataWriter.Edit(startJob);
                }
                else
                {
                    startJob.Trigger = FindNextOccurrence(endJob.Trigger, user.WeekStartDay, user.WeekStartHour);
                    _dataWriter.Edit(startJob);
                }
            }
        }

        public void ScheduleEndJob(string userId)
        {
            var startJob = _dataReader.GetAll<Job>()
                .Where(i => i.UserId.Equals(userId, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault(i => i.Processor.Equals(typeof(StartWeekJob).Name, StringComparison.InvariantCultureIgnoreCase));
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

        private DateTime FindNextOccurrence(DateTime start, DayOfWeek day, int hour)
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