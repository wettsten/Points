using System;
using System.Linq;
using Points.Data.Raven;
using Points.DataAccess;
using Points.Scheduler.Factories;
using Points.Scheduler.Jobs;

namespace Points.Scheduler.Processors
{
    public class JobProcessor : IJobProcessor
    {
        private readonly IDataReader _dataReader;
        private readonly IDataWriter _dataWriter;
        private readonly IJobFactory _jobFactory;

        public JobProcessor(IDataReader dataReader, IDataWriter dataWriter, IJobFactory jobFactory)
        {
            _dataReader = dataReader;
            _dataWriter = dataWriter;
            _jobFactory = jobFactory;
        }

        public void ProcessJobs()
        {
            var jobQ = _dataReader
                .GetAll<Job>()
                .Where(i => !i.IsDeleted)
                .Where(i => i.Trigger < DateTime.UtcNow.AddMinutes(1))
                .OrderBy(i => i.Trigger);
            foreach (var job in jobQ)
            {
                var iJob = _jobFactory.GetJobProcessor(job.Processor);
                for (int i = 0; i < 2; i++)
                {
                    try
                    {
                        iJob.Process(job);
                        break;
                    }
                    catch (Exception ex)
                    {
                        // maybe update job to show an error?
                    }
                }
            }
        }

        public void ScheduleStartJob(string userId)
        {
            var user = _dataReader.Get<User>(userId);
            var startJob = _dataReader.GetAll<Job>()
                .Where(i => i.UserId.Equals(user.Id, StringComparison.InvariantCultureIgnoreCase))
                .Where(i => i.Processor.Equals(typeof(StartWeekJob).Name, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault(i => !i.IsDeleted);
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
                    .Where(i => i.Processor.Equals(typeof(EndWeekJob).Name, StringComparison.InvariantCultureIgnoreCase))
                    .FirstOrDefault(i => !i.IsDeleted);
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