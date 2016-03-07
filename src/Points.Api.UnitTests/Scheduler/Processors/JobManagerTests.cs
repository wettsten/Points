using System;
using System.Collections.Generic;
using AutoMoq.Helpers;
using Moq;
using NUnit.Framework;
using Points.Data;
using Points.DataAccess.Readers;
using Points.DataAccess.Writers;
using Points.Scheduler.Processors;
using Shouldly;

namespace Points.Api.UnitTests.Scheduler.Processors
{
    [TestFixture]
    public class JobManagerTests : AutoMoqTestFixture<JobManager>
    {
        private User _user;
        private List<Job> _jobs;

        [TestFixtureSetUp]
        public void Setup()
        {
            var userId = Guido.New();
            _user = new User
            {
                Id = userId,
                UserId = userId,
                WeekStartDay = DayOfWeek.Friday,
                WeekStartHour = 12
            };
        }

        [SetUp]
        public void TestSetup()
        {
            ResetSubject();
            _jobs = new List<Job>
            {
                new Job
                {
                    Id = Guido.New(),
                    Name = "job1",
                    UserId = _user.Id,
                    Processor = "StartWeekJob"
                },
                new Job
                {
                    Id = Guido.New(),
                    Name = "job1",
                    UserId = Guido.New(),
                    Processor = "StartWeekJob"
                },
                new Job
                {
                    Id = Guido.New(),
                    Name = "job1",
                    UserId = _user.Id,
                    Processor = "OtherJob"
                }
            };
            Mocked<ISingleSessionDataReader>().Setup(r => r.GetAll<Job>()).Returns(_jobs);
            Mocked<ISingleSessionDataReader>().Setup(r => r.Get<User>(_user.Id)).Returns(_user);
        }

        [Test]
        public void ScheduleStartJobDoesNotAlreadyExistAddIsCalled()
        {
            _jobs.RemoveAt(0);
            Mocked<ISingleSessionDataReader>().Setup(r => r.GetAll<Job>()).Returns(_jobs);

            Subject.ScheduleStartJob(_user.Id);

            Mocked<ISingleSessionDataWriter>().Verify(r => r.Add(It.IsAny<Job>()), Times.Once());
        }

        [Test]
        public void ScheduleStartJobDoesNotAlreadyExistData()
        {
            _jobs.RemoveAt(0);
            Mocked<ISingleSessionDataReader>().Setup(r => r.GetAll<Job>()).Returns(_jobs);
            Job job = null;
            Mocked<ISingleSessionDataWriter>()
                .Setup(r => r.Add(It.IsAny<Job>()))
                .Callback<Job>(r => job = r);

            Subject.ScheduleStartJob(_user.Id);

            job.ShouldSatisfyAllConditions(
                () => job.Id.ShouldBeEmpty(),
                () => job.Processor.ShouldBe("StartWeekJob"),
                () => job.UserId.ShouldBe(_user.Id),
                () => job.Trigger.ShouldBe(Subject.FindNextOccurrence(DateTime.UtcNow, _user.WeekStartDay, _user.WeekStartHour)));
        }

        [Test]
        public void ScheduleStartJobExistsEndJobDoesNotExistEditIsCalled()
        {
            Subject.ScheduleStartJob(_user.Id);

            Mocked<ISingleSessionDataWriter>().Verify(r => r.Edit(It.IsAny<Job>()), Times.Once());
        }

        [Test]
        public void ScheduleStartJobExistsEndJobDoesNotExistTriggerIsUpdated()
        {
            Job job = null;
            Mocked<ISingleSessionDataWriter>()
                .Setup(r => r.Edit(It.IsAny<Job>()))
                .Callback<Job>(r => job = r);

            Subject.ScheduleStartJob(_user.Id);

            job.Trigger.ShouldBe(Subject.FindNextOccurrence(DateTime.UtcNow, _user.WeekStartDay, _user.WeekStartHour));
        }

        [Test]
        public void ScheduleStartJobExistsEndJobExistsEditIsCalled()
        {
            _jobs.Add(new Job
            {
                Id = Guido.New(),
                Name = "endjob",
                UserId = _user.Id,
                Processor = "EndWeekJob",
                Trigger = new DateTime(2016, 1, 1)
            });
            Mocked<ISingleSessionDataReader>().Setup(r => r.GetAll<Job>()).Returns(_jobs);

            Subject.ScheduleStartJob(_user.Id);

            Mocked<ISingleSessionDataWriter>().Verify(r => r.Edit(It.IsAny<Job>()), Times.Once());
        }

        [Test]
        public void ScheduleStartJobExistsEndJobExistsTriggerIsUpdated()
        {
            _jobs.Add(new Job
            {
                Id = Guido.New(),
                Name = "endjob",
                UserId = _user.Id,
                Processor = "EndWeekJob",
                Trigger = new DateTime(2016, 1, 1)
            });
            Mocked<ISingleSessionDataReader>().Setup(r => r.GetAll<Job>()).Returns(_jobs);
            Job job = null;
            Mocked<ISingleSessionDataWriter>()
                .Setup(r => r.Edit(It.IsAny<Job>()))
                .Callback<Job>(r => job = r);

            Subject.ScheduleStartJob(_user.Id);

            job.Trigger.ShouldBe(Subject.FindNextOccurrence(new DateTime(2016, 1, 1), _user.WeekStartDay, _user.WeekStartHour));
        }

        [Test]
        public void ScheduleEndJobJobIsCreated()
        {
            Subject.ScheduleEndJob(_user.UserId);

            Mocked<ISingleSessionDataWriter>().Verify(r => r.Add<Job>(It.IsAny<Job>()), Times.Once());
        }

        [Test]
        public void ScheduleEndJobNoJobsAtAllFoundNoJobIsCreated()
        {
            Mocked<ISingleSessionDataReader>().Setup(r => r.GetAll<Job>()).Returns(new List<Job>());

            Subject.ScheduleEndJob(_user.UserId);

            Mocked<ISingleSessionDataWriter>().Verify(r => r.Add<Job>(It.IsAny<Job>()), Times.Never());
        }

        [Test]
        public void ScheduleEndJobNoStartJobFoundNoJobIsCreated()
        {
            _jobs.RemoveAt(0);
            Mocked<ISingleSessionDataReader>().Setup(r => r.GetAll<Job>()).Returns(_jobs);

            Subject.ScheduleEndJob(_user.UserId);

            Mocked<ISingleSessionDataWriter>().Verify(r => r.Add<Job>(It.IsAny<Job>()), Times.Never());
        }

        [Test]
        public void ScheduleEndJobData()
        {
            Job job = null;
            Mocked<ISingleSessionDataWriter>()
                .Setup(r => r.Add(It.IsAny<Job>()))
                .Callback<Job>(r => job = r);

            Subject.ScheduleEndJob(_user.UserId);

            job.ShouldSatisfyAllConditions(
                () => job.Id.ShouldBeEmpty(),
                () => job.Processor.ShouldBe("EndWeekJob"),
                () => job.UserId.ShouldBe(_user.Id),
                () => job.Trigger.ShouldBe(_jobs[0].Trigger.AddDays(7)));
        }

        [Test]
        public void FindNextOccurrenceDifferentDay()
        {
            var start = new DateTime(2016, 1, 1, 12, 0, 0);
            var result = Subject.FindNextOccurrence(start, DayOfWeek.Monday, 18);
            result.ShouldSatisfyAllConditions(
                () => result.Year.ShouldBe(2016),
                () => result.Month.ShouldBe(1),
                () => result.Day.ShouldBe(4),
                () => result.Hour.ShouldBe(18));
        }

        [Test]
        public void FindNextOccurrenceSameDayHourIsLater()
        {
            var start = new DateTime(2016, 1, 1, 12, 0, 0);
            var result = Subject.FindNextOccurrence(start, DayOfWeek.Friday, 18);
            result.ShouldSatisfyAllConditions(
                () => result.Year.ShouldBe(2016),
                () => result.Month.ShouldBe(1),
                () => result.Day.ShouldBe(1),
                () => result.Hour.ShouldBe(18));
        }

        [Test]
        public void FindNextOccurrenceSameDayHourIsEarlier()
        {
            var start = new DateTime(2016, 1, 1, 12, 0, 0);
            var result = Subject.FindNextOccurrence(start, DayOfWeek.Friday, 6);
            result.ShouldSatisfyAllConditions(
                () => result.Year.ShouldBe(2016),
                () => result.Month.ShouldBe(1),
                () => result.Day.ShouldBe(8),
                () => result.Hour.ShouldBe(6));
        }
    }
}