using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using NLog;
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
        private Timer _hourTimer;
        private string _keepAliveUrl;
        private readonly ILogger _logger = LogManager.GetLogger("Scheduler");

        public Scheduler(ISingleSessionDataReader dataReader, IJobFactory jobFactory, ISingleSessionDataWriter dataWriter)
        {
            _dataReader = dataReader;
            _jobFactory = jobFactory;
            _dataWriter = dataWriter;
        }

        public void Start(string keepAliveUrl)
        {
            _keepAliveUrl = keepAliveUrl;
            _logger.Info("Keep Alive Url: " + _keepAliveUrl);
            _hourTimer = new Timer(HourTick);
            _logger.Info("Scheduler starting up");
            HourTick(null);
            var now = DateTime.UtcNow;
            var ts = new DateTime(now.Year, now.Month, now.Day, now.Hour + 1, 0, 0) - DateTime.UtcNow;
            _hourTimer.Change(ts, TimeSpan.FromHours(1));
        }

        internal void HourTick(object t)
        {
            _logger.Info("Scheduler processing jobs");

            KeepAlive();

            var jobQ = _dataReader
                .GetAll<Job>()
                .Where(i => i.Trigger < DateTime.UtcNow.AddMinutes(1))
                .OrderBy(i => i.Trigger);

            _logger.Debug("Scheduler found {0} jobs to process", jobQ.Count());

            foreach (var job in jobQ)
            {
                _logger.Debug("Scheduler processing job {0}", job.Id);
                var iJob = _jobFactory.GetJobProcessor(job.Processor);
                try
                {
                    iJob.Process(job);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Scheduler error processing job: {0}", job.Id);
                }
                _dataWriter.Delete<Job>(job.Id);
            }

            _logger.Debug("Scheduler finished processing jobs");
        }

        internal async void KeepAlive()
        {
            var client = new HttpClient();
            var uri = new Uri(_keepAliveUrl);
            var response = await client.GetAsync(uri);
        }
    }
}