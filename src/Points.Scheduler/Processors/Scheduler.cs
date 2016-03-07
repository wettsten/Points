
using System;
using System.Linq;
using System.Threading;
using Points.Data;
using Points.DataAccess.Readers;
using Points.DataAccess.Writers;
using Points.Scheduler.Factories;

namespace Points.Scheduler.Processors
{
    public class Scheduler : IScheduler
    {
        private readonly ISingleSessionDataReader _dataReader;
        private readonly ISingleSessionDataWriter _dataWriter;
        private readonly IJobFactory _jobFactory;
        private readonly Timer _hourTimer;

        public Scheduler(ISingleSessionDataReader dataReader, IJobFactory jobFactory, ISingleSessionDataWriter dataWriter)
        {
            _dataReader = dataReader;
            _jobFactory = jobFactory;
            _dataWriter = dataWriter;
            _hourTimer = new Timer(HourTick);
        }

        public void Start()
        {
            HourTick(null);
            var now = DateTime.UtcNow.AddHours(1);
            var ts = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0) - DateTime.UtcNow;
            _hourTimer.Change(ts, TimeSpan.FromHours(1));
        }

        internal void HourTick(object t)
        {
            var jobQ = _dataReader
                .GetAll<Job>()
                .Where(i => i.Trigger < DateTime.UtcNow.AddMinutes(1))
                .OrderBy(i => i.Trigger);
            foreach (var job in jobQ)
            {
                var iJob = _jobFactory.GetJobProcessor(job.Processor);
                try
                {
                    iJob.Process(job);
                }
                catch (Exception ex)
                {
                    // maybe update job to show an error?
                }
                _dataWriter.Delete<Job>(job.Id);
            }
        }
    }
}