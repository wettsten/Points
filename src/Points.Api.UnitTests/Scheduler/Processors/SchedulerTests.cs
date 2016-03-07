using System;
using System.Collections.Generic;
using AutoMoq.Helpers;
using Moq;
using NUnit.Framework;
using Points.Data;
using Points.DataAccess.Readers;
using Points.DataAccess.Writers;
using Points.Scheduler.Factories;
using Points.Scheduler;

namespace Points.Api.UnitTests.Scheduler.Processors
{
    [TestFixture]
    public class SchedulerTests : AutoMoqTestFixture<Points.Scheduler.Processors.Scheduler>
    {
        private List<Job> _jobs;
        private Mock<IJob> _mockIJob;
            
        [TestFixtureSetUp]
        public void Setup()
        {
            _jobs = new List<Job>
            {
                new Job
                {
                    Name = "job1",
                    Trigger = DateTime.UtcNow.AddMinutes(2)
                },
                new Job
                {
                    Name = "job2",
                    Trigger = DateTime.UtcNow.AddMinutes(-1)
                },
                new Job
                {
                    Name = "job3",
                    Trigger = DateTime.UtcNow.AddMinutes(-10)
                }
            };
        }

        [SetUp]
        public void TestSetup()
        {
            ResetSubject();
            _mockIJob = new Mock<IJob>();
            Mocked<ISingleSessionDataReader>().Setup(r => r.GetAll<Job>()).Returns(_jobs);
            Mocked<IJobFactory>()
                .Setup(r => r.GetJobProcessor(It.IsAny<string>()))
                .Returns(_mockIJob.Object);
        }

        [Test]
        public void TwoJobsAreFoundHandlers()
        {
            Subject.HourTick(null);

            Mocked<IJobFactory>().Verify(r => r.GetJobProcessor(It.IsAny<string>()), Times.Exactly(2));
        }
        
        [Test]
        public void TwoJobsAreProcessed()
        {
            Subject.HourTick(null);

            _mockIJob.Verify(r => r.Process(It.IsAny<Job>()), Times.Exactly(2));
        }

        [Test]
        public void TwoJobsAreDeleted()
        {
            Subject.HourTick(null);

            Mocked<ISingleSessionDataWriter>().Verify(r => r.Delete<Job>(It.IsAny<string>()), Times.Exactly(2));
        }
    }
}