using System;
using System.Collections.Generic;
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
                    TaskId = Guido.New(),
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

        [Test]
        public void ValuesAreCopied()
        {
            ArchivedTask arcTask = null;
            Mocked<ISingleSessionDataWriter>()
                .Setup(r => r.Add(It.IsAny<ArchivedTask>()))
                .Callback<ArchivedTask>(r => arcTask = r);

            Subject.Process(_context);

            arcTask.ShouldSatisfyAllConditions(
                () => arcTask.Id.ShouldBeEmpty(),
                () => arcTask.Name.ShouldBe(_activeTasks[1].Name),
                () => arcTask.UserId.ShouldBe(_activeTasks[1].UserId),
                () => arcTask.DateStarted.ShouldBe(_activeTasks[1].DateStarted),
                () => arcTask.TaskId.ShouldBe(_activeTasks[1].TaskId),
                () => arcTask.TimesCompleted.ShouldBe(_activeTasks[1].TimesCompleted),
                () => arcTask.Duration.Type.ShouldBe(_activeTasks[1].Duration.Type),
                () => arcTask.Duration.Value.ShouldBe(_activeTasks[1].Duration.Value),
                () => arcTask.Duration.Unit.ShouldBe(_activeTasks[1].Duration.Unit),
                () => arcTask.Frequency.Type.ShouldBe(_activeTasks[1].Frequency.Type),
                () => arcTask.Frequency.Value.ShouldBe(_activeTasks[1].Frequency.Value),
                () => arcTask.Frequency.Unit.ShouldBe(_activeTasks[1].Frequency.Unit),
                () => arcTask.DateEnded.ShouldBeGreaterThan(DateTime.UtcNow.AddSeconds(-1)),
                () => arcTask.DateEnded.ShouldBeLessThan(DateTime.UtcNow));
        }
    }
}