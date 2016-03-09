using System;
using System.Collections.Generic;
using AutoMapper;
using AutoMoq.Helpers;
using Moq;
using NUnit.Framework;
using Points.Scheduler.Jobs;
using Points.Data;
using Points.DataAccess.Readers;
using Points.DataAccess.Writers;
using Shouldly;

namespace Points.Api.UnitTests.Scheduler.Jobs
{
    public class EndWeekJobTests : AutoMoqTestFixture<EndWeekJob>
    {
        private Job _context;
        private List<ActiveTask> _activeTasks;

        [TestFixtureSetUp]
        public void Setup()
        {
            _context = new Job
            {
                Id = Guido.New(),
                Name = "job",
                Processor = "EndWeekJob",
                Trigger = new DateTime(),
                UserId = Guido.New()
            };

            _activeTasks = new List<ActiveTask>
            {
                new ActiveTask { UserId = _context.UserId },
                new ActiveTask
                {
                    UserId = _context.UserId,
                    Id = Guido.New(),
                    Name = "active1",
                    DateStarted = DateTime.UtcNow,
                    Duration = new Duration
                    {
                        Type = DurationType.AtLeast,
                        Value = 1,
                        Unit = DurationUnit.Hours
                    },
                    Frequency = new Frequency
                    {
                        Type = FrequencyType.AtLeast,
                        Value = 2,
                        Unit = FrequencyUnit.Times
                    },
                    TaskName = "task",
                    CategoryName = "cat",
                    TimesCompleted = 3
                },
                new ActiveTask { UserId = Guido.New() }
            };
        }

        [SetUp]
        public void TestSetup()
        {
            ResetSubject();
            Mocked<ISingleSessionDataReader>().Setup(r => r.GetAll<ActiveTask>()).Returns(_activeTasks);
        }

        [Test]
        public void OnlyCopiesUsersTasks()
        {
            Subject.Process(_context);

            Mocked<ISingleSessionDataWriter>().Verify(r => r.Add(It.IsAny<ArchivedTask>()), Times.Exactly(2));
            Mocked<ISingleSessionDataWriter>().Verify(r => r.Delete<ActiveTask>(It.IsAny<string>()), Times.Exactly(2));
        }

        [Test]
        public void NoActiveTasksDoesNothing()
        {
            Mocked<ISingleSessionDataReader>().Setup(r => r.GetAll<ActiveTask>()).Returns(new List<ActiveTask>());

            Subject.Process(_context);

            Mocked<ISingleSessionDataWriter>().Verify(r => r.Add(It.IsAny<ArchivedTask>()), Times.Never());
            Mocked<ISingleSessionDataWriter>().Verify(r => r.Delete<ActiveTask>(It.IsAny<string>()), Times.Never());
        }
    }
}