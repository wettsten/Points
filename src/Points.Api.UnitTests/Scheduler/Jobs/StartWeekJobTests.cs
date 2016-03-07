using System;
using System.Collections.Generic;
using AutoMoq.Helpers;
using Moq;
using NUnit.Framework;
using Points.Scheduler.Jobs;
using Points.Data;
using Points.DataAccess.Readers;
using Points.DataAccess.Writers;
using Points.Scheduler.Processors;
using Shouldly;

namespace Points.Api.UnitTests.Scheduler.Jobs
{
    public class StartWeekJobTests : AutoMoqTestFixture<StartWeekJob>
    {
        private Job _context;
        private List<PlanningTask> _planningTasks;
        private User _user;

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

            _planningTasks = new List<PlanningTask>
            {
                new PlanningTask { UserId = _context.UserId },
                new PlanningTask
                {
                    UserId = _context.UserId,
                    Id = Guido.New(),
                    Name = "planning1",
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
                    TaskId = Guido.New()
                },
                new PlanningTask { UserId = Guido.New() }
            };

            _user = new User
            {
                Id = _context.UserId,
                UserId = _context.UserId,
                TargetPoints = 10,
                ActiveTargetPoints = 0
            };
        }

        [SetUp]
        public void TestSetup()
        {
            ResetSubject();
            Mocked<ISingleSessionDataReader>().Setup(r => r.GetAll<PlanningTask>()).Returns(_planningTasks);
            Mocked<ISingleSessionDataReader>().Setup(r => r.Get<User>(_context.UserId)).Returns(_user);
        }

        [Test]
        public void OnlyCopiesUsersTasks()
        {
            Subject.Process(_context);

            Mocked<ISingleSessionDataWriter>().Verify(r => r.Add(It.IsAny<ActiveTask>()), Times.Exactly(2));
        }

        [Test]
        public void JobManagerSchedulesEndJobOnlyOnce()
        {
            Subject.Process(_context);

            Mocked<IJobManager>().Verify(r => r.ScheduleEndJob(_context.UserId), Times.Once());
        }

        [Test]
        public void UserTargetPointsEditedOnlyOnce()
        {
            Subject.Process(_context);

            Mocked<ISingleSessionDataWriter>().Verify(r => r.Edit(It.IsAny<User>()), Times.Once());
        }

        [Test]
        public void JobManagerSchedulesNextStartJobOnlyOnce()
        {
            Subject.Process(_context);

            Mocked<IJobManager>().Verify(r => r.ScheduleStartJob(_context.UserId), Times.Once());
        }

        [Test]
        public void NoPlanningTasksDoesNotAddActiveTasks()
        {
            Mocked<ISingleSessionDataReader>().Setup(r => r.GetAll<PlanningTask>()).Returns(new List<PlanningTask>());

            Subject.Process(_context);

            Mocked<ISingleSessionDataWriter>().Verify(r => r.Add(It.IsAny<ActiveTask>()), Times.Never());
        }

        [Test]
        public void NoPlanningTasksDoesNotScheduleEndJob()
        {
            Mocked<ISingleSessionDataReader>().Setup(r => r.GetAll<PlanningTask>()).Returns(new List<PlanningTask>());

            Subject.Process(_context);
            
            Mocked<IJobManager>().Verify(r => r.ScheduleEndJob(_context.UserId), Times.Never());
        }

        [Test]
        public void NoPlanningTasksDoesNotEditUser()
        {
            Mocked<ISingleSessionDataReader>().Setup(r => r.GetAll<PlanningTask>()).Returns(new List<PlanningTask>());

            Subject.Process(_context);

            Mocked<ISingleSessionDataWriter>().Verify(r => r.Edit(It.IsAny<User>()), Times.Never());
        }

        [Test]
        public void NoPlanningTasksStillSchedulesNextStartJobOnlyOnce()
        {
            Subject.Process(_context);

            Mocked<IJobManager>().Verify(r => r.ScheduleStartJob(_context.UserId), Times.Once());
        }

        [Test]
        public void ValuesAreCopied()
        {
            ActiveTask actTask = null;
            Mocked<ISingleSessionDataWriter>()
                .Setup(r => r.Add(It.IsAny<ActiveTask>()))
                .Callback<ActiveTask>(r => actTask = r);

            Subject.Process(_context);

            actTask.ShouldSatisfyAllConditions(
                () => actTask.Id.ShouldBeEmpty(),
                () => actTask.Name.ShouldBe(_planningTasks[1].Name),
                () => actTask.UserId.ShouldBe(_planningTasks[1].UserId),
                () => actTask.TaskId.ShouldBe(_planningTasks[1].TaskId),
                () => actTask.TimesCompleted.ShouldBe(0),
                () => actTask.Duration.Type.ShouldBe(_planningTasks[1].Duration.Type),
                () => actTask.Duration.Value.ShouldBe(_planningTasks[1].Duration.Value),
                () => actTask.Duration.Unit.ShouldBe(_planningTasks[1].Duration.Unit),
                () => actTask.Frequency.Type.ShouldBe(_planningTasks[1].Frequency.Type),
                () => actTask.Frequency.Value.ShouldBe(_planningTasks[1].Frequency.Value),
                () => actTask.Frequency.Unit.ShouldBe(_planningTasks[1].Frequency.Unit),
                () => actTask.DateStarted.ShouldBeGreaterThan(DateTime.UtcNow.AddSeconds(-1)),
                () => actTask.DateStarted.ShouldBeLessThan(DateTime.UtcNow));
        }

        [Test]
        public void UserIsEdited()
        {
            User editUser = null;
            Mocked<ISingleSessionDataWriter>()
                .Setup(r => r.Edit(It.IsAny<User>()))
                .Callback<User>(r => editUser = r);

            Subject.Process(_context);

            editUser.ActiveTargetPoints.ShouldBe(10);
        }
    }
}