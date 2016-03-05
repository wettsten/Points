using System;
using System.Collections.Generic;
using AutoMapper;
using AutoMoq.Helpers;
using NUnit.Framework;
using Points.Common.AutoMapper;
using Points.Common.EnumExtensions;
using Shouldly;
using Points.DataAccess.Readers;

namespace Points.Api.UnitTests.Common.AutoMapper
{
    [TestFixture]
    public class DataToModelMapperTests : AutoMoqTestFixture<MappingProfile>
    {
        private IMapper _mapper;

        private Data.Duration _duration;
        private Data.Frequency _frequency;
        private Data.Category _cat;
        private Data.Task _task;
        private Data.PlanningTask _pTask;
        private Data.ActiveTask _aTask;
        private Data.ArchivedTask _arcTask;
        private Data.Job _job;
        private Data.User _user;

        [OneTimeSetUp]
        [TestFixtureSetUp]
        public void TestDataSetup()
        {
            _mapper = new MapperConfiguration(mapper => mapper.AddProfile(Subject)).CreateMapper();
            
            SetupData();
        }

        private void SetupData()
        {
            _duration = new Data.Duration
            {
                Type = Data.DurationType.AtLeast,
                Value = 1,
                Unit = Data.DurationUnit.Hours
            };
            _frequency = new Data.Frequency
            {
                Type = Data.FrequencyType.AtLeast,
                Value = 1,
                Unit = Data.FrequencyUnit.Times
            };
            _cat = new Data.Category
            {
                Id = Guid.NewGuid().ToString("D"),
                Name = "catName"
            };
            _task = new Data.Task
            {
                Id = Guid.NewGuid().ToString("D"),
                Name = "taskName",
                CategoryId = _cat.Id
            };
            _pTask = new Data.PlanningTask
            {
                Id = Guid.NewGuid().ToString("D"),
                Name = "pTaskName",
                TaskId = _task.Id,
                Duration = _duration,
                Frequency = _frequency
            };
            _aTask = new Data.ActiveTask
            {
                Id = Guid.NewGuid().ToString("D"),
                Name = "aTaskName",
                TaskId = _task.Id,
                Duration = _duration,
                Frequency = _frequency,
                DateStarted = DateTime.UtcNow,
                TimesCompleted = 3
            };
            _arcTask = new Data.ArchivedTask
            {
                Id = Guid.NewGuid().ToString("D"),
                Name = "arcTaskName",
                TaskId = _task.Id,
                Duration = _duration,
                Frequency = _frequency,
                DateStarted = DateTime.UtcNow,
                TimesCompleted = 3,
                DateEnded = DateTime.UtcNow.AddMinutes(1)
            };
            _job = new Data.Job
            {
                Id = Guid.NewGuid().ToString("D"),
                Name = "jobName",
                Processor = "jobProcessor",
                Trigger = DateTime.UtcNow
            };
            _user = new Data.User
            {
                Id = Guid.NewGuid().ToString("D"),
                Name = "userName",
                Email = "a@a",
                TargetPoints = 10,
                WeekSummaryEmail = true,
                WeekStartDay = DayOfWeek.Friday,
                WeekStartHour = 12,
                NotifyWeekStarting = 2,
                NotifyWeekEnding = 3,
                EnableAdvancedFeatures = true,
                ActiveTargetPoints = 15,
                CategoryBonus = 1,
                TaskMultiplier = 2,
                BonusPointMultiplier = 3,
                DurationBonusPointsPerHour = 2
            };

            Mocked<IDataReader>().Setup(r => r.Get<Data.Category>(_task.CategoryId)).Returns(_cat);
            Mocked<IDataReader>().Setup(r => r.Get<Data.Task>(_pTask.TaskId)).Returns(_task);
            Mocked<IDataReader>().Setup(r => r.GetAll<Data.Job>()).Returns(new List<Data.Job>());
        }

        [Test]
        public void RavenObjectToViewObject()
        {
            var obj = new Data.RavenObject
            {
                Id = Guid.NewGuid().ToString("D"),
                Name = "name"
            };

            var model = _mapper.Map<Data.RavenObject, Model.ViewObject>(obj);

            model.ShouldSatisfyAllConditions(
                () => model.Id.ShouldBe(obj.Id),
                () => model.Name.ShouldBe(obj.Name));
        }

        [Test]
        public void RavenDurationToViewDuration()
        {
            var model = _mapper.Map<Data.Duration, Model.Duration>(_duration);

            model.ShouldSatisfyAllConditions(
                () => model.Type.Id.ShouldBe(_duration.Type.ToString()),
                () => model.Type.Name.ShouldBe(_duration.Type.ToString().Spacify()),
                () => model.Value.ShouldBe(_duration.Value),
                () => model.Unit.Id.ShouldBe(_duration.Unit.ToString()),
                () => model.Unit.Name.ShouldBe(_duration.Unit.ToString().Spacify()));
        }

        [Test]
        public void RavenFrequencyToViewFrequency()
        {
            var model = _mapper.Map<Data.Frequency, Model.Frequency>(_frequency);

            model.ShouldSatisfyAllConditions(
                () => model.Type.Id.ShouldBe(_frequency.Type.ToString()),
                () => model.Type.Name.ShouldBe(_frequency.Type.ToString().Spacify()),
                () => model.Value.ShouldBe(_frequency.Value),
                () => model.Unit.Id.ShouldBe(_frequency.Unit.ToString()),
                () => model.Unit.Name.ShouldBe(_frequency.Unit.ToString().Spacify()));
        }

        [Test]
        public void RavenCategoryToViewCategory()
        {
            var model = _mapper.Map<Data.Category, Model.Category>(_cat);

            model.ShouldSatisfyAllConditions(
                () => model.Id.ShouldBe(_cat.Id),
                () => model.Name.ShouldBe(_cat.Name));
        }

        [Test]
        public void RavenTaskToViewTask()
        {
            var model = _mapper.Map<Data.Task, Model.Task>(_task);

            model.ShouldSatisfyAllConditions(
                () => model.Id.ShouldBe(_task.Id),
                () => model.Name.ShouldBe(_task.Name),
                () => model.Category.Id.ShouldBe(_cat.Id),
                () => model.Category.Name.ShouldBe(_cat.Name));
        }

        [Test]
        public void RavenPlanningTaskToViewPlanningTask()
        {
            var model = _mapper.Map<Data.PlanningTask, Model.PlanningTask>(_pTask);

            model.ShouldSatisfyAllConditions(
                () => model.Id.ShouldBe(_pTask.Id),
                () => model.Name.ShouldBe(_pTask.Name),
                () => model.Task.Id.ShouldBe(_task.Id),
                () => model.Task.Name.ShouldBe(_task.Name),
                () => model.Task.Category.Id.ShouldBe(_cat.Id),
                () => model.Task.Category.Name.ShouldBe(_cat.Name),
                () => model.Duration.Type.Id.ShouldBe(_duration.Type.ToString()),
                () => model.Duration.Type.Name.ShouldBe(_duration.Type.ToString().Spacify()),
                () => model.Duration.Value.ShouldBe(_duration.Value),
                () => model.Duration.Unit.Id.ShouldBe(_duration.Unit.ToString()),
                () => model.Duration.Unit.Name.ShouldBe(_duration.Unit.ToString().Spacify()),
                () => model.Frequency.Type.Id.ShouldBe(_frequency.Type.ToString()),
                () => model.Frequency.Type.Name.ShouldBe(_frequency.Type.ToString().Spacify()),
                () => model.Frequency.Value.ShouldBe(_frequency.Value),
                () => model.Frequency.Unit.Id.ShouldBe(_frequency.Unit.ToString()),
                () => model.Frequency.Unit.Name.ShouldBe(_frequency.Unit.ToString().Spacify()));
        }

        [Test]
        public void RavenActiveTaskToViewActiveTask()
        {
            var model = _mapper.Map<Data.ActiveTask, Model.ActiveTask>(_aTask);

            model.ShouldSatisfyAllConditions(
                () => model.Id.ShouldBe(_aTask.Id),
                () => model.Name.ShouldBe(_aTask.Name),
                () => model.Task.Id.ShouldBe(_task.Id),
                () => model.Task.Name.ShouldBe(_task.Name),
                () => model.Task.Category.Id.ShouldBe(_cat.Id),
                () => model.Task.Category.Name.ShouldBe(_cat.Name),
                () => model.DateStarted.ShouldBe(_aTask.DateStarted),
                () => model.TimesCompleted.ShouldBe(_aTask.TimesCompleted),
                () => model.Duration.Type.Id.ShouldBe(_duration.Type.ToString()),
                () => model.Duration.Type.Name.ShouldBe(_duration.Type.ToString().Spacify()),
                () => model.Duration.Value.ShouldBe(_duration.Value),
                () => model.Duration.Unit.Id.ShouldBe(_duration.Unit.ToString()),
                () => model.Duration.Unit.Name.ShouldBe(_duration.Unit.ToString().Spacify()),
                () => model.Frequency.Type.Id.ShouldBe(_frequency.Type.ToString()),
                () => model.Frequency.Type.Name.ShouldBe(_frequency.Type.ToString().Spacify()),
                () => model.Frequency.Value.ShouldBe(_frequency.Value),
                () => model.Frequency.Unit.Id.ShouldBe(_frequency.Unit.ToString()),
                () => model.Frequency.Unit.Name.ShouldBe(_frequency.Unit.ToString().Spacify()));
        }

        [Test]
        public void RavenArchivedTaskToViewArchivedTask()
        {
            var model = _mapper.Map<Data.ArchivedTask, Model.ArchivedTask>(_arcTask);

            model.ShouldSatisfyAllConditions(
                () => model.Id.ShouldBe(_arcTask.Id),
                () => model.Name.ShouldBe(_arcTask.Name),
                () => model.Task.Id.ShouldBe(_task.Id),
                () => model.Task.Name.ShouldBe(_task.Name),
                () => model.Task.Category.Id.ShouldBe(_cat.Id),
                () => model.Task.Category.Name.ShouldBe(_cat.Name),
                () => model.DateStarted.ShouldBe(_arcTask.DateStarted),
                () => model.TimesCompleted.ShouldBe(_arcTask.TimesCompleted),
                () => model.DateEnded.ShouldBe(_arcTask.DateEnded),
                () => model.Duration.Type.Id.ShouldBe(_duration.Type.ToString()),
                () => model.Duration.Type.Name.ShouldBe(_duration.Type.ToString().Spacify()),
                () => model.Duration.Value.ShouldBe(_duration.Value),
                () => model.Duration.Unit.Id.ShouldBe(_duration.Unit.ToString()),
                () => model.Duration.Unit.Name.ShouldBe(_duration.Unit.ToString().Spacify()),
                () => model.Frequency.Type.Id.ShouldBe(_frequency.Type.ToString()),
                () => model.Frequency.Type.Name.ShouldBe(_frequency.Type.ToString().Spacify()),
                () => model.Frequency.Value.ShouldBe(_frequency.Value),
                () => model.Frequency.Unit.Id.ShouldBe(_frequency.Unit.ToString()),
                () => model.Frequency.Unit.Name.ShouldBe(_frequency.Unit.ToString().Spacify()));
        }

        [Test]
        public void RavenJobToViewJob()
        {
            var model = _mapper.Map<Data.Job, Model.Job>(_job);

            model.ShouldSatisfyAllConditions(
                () => model.Id.ShouldBe(_job.Id),
                () => model.Name.ShouldBe(_job.Name),
                () => model.Processor.ShouldBe(_job.Processor),
                () => model.Trigger.ShouldBe(_job.Trigger));
        }

        [Test]
        public void RavenUserToViewUser()
        {
            var model = _mapper.Map<Data.User, Model.User>(_user);

            model.ShouldSatisfyAllConditions(
                () => model.Id.ShouldBe(_user.Id),
                () => model.Name.ShouldBe(_user.Name),
                () => model.Email.ShouldBe(_user.Email),
                () => model.TargetPoints.ShouldBe(_user.TargetPoints),
                () => model.WeekSummaryEmail.ShouldBe(_user.WeekSummaryEmail),
                () => model.WeekStartDay.ShouldBe(_user.WeekStartDay),
                () => model.WeekStartHour.Id.ShouldBe(_user.WeekStartHour),
                () => model.WeekStartHour.Name.ShouldBeNull(),
                () => model.NotifyWeekStarting.Id.ShouldBe(_user.NotifyWeekStarting),
                () => model.NotifyWeekStarting.Name.ShouldBeNull(),
                () => model.NotifyWeekEnding.Id.ShouldBe(_user.NotifyWeekEnding),
                () => model.NotifyWeekEnding.Name.ShouldBeNull(),
                () => model.EnableAdvancedFeatures.ShouldBe(_user.EnableAdvancedFeatures),
                () => model.ActiveTargetPoints.ShouldBe(_user.ActiveTargetPoints),
                () => model.CategoryBonus.ShouldBe(_user.CategoryBonus),
                () => model.TaskMultiplier.ShouldBe(_user.TaskMultiplier),
                () => model.BonusPointMultiplier.ShouldBe(_user.BonusPointMultiplier),
                () => model.DurationBonusPointsPerHour.ShouldBe(_user.DurationBonusPointsPerHour));
        }

        [Test]
        public void RavenUserLookupJobs()
        {
            var jobs = new List<Data.Job>
            {
                new Data.Job
                {
                    Id = Guid.NewGuid().ToString("D"),
                    Name = "job1",
                    UserId = _user.Id,
                    Trigger = DateTime.UtcNow,
                    Processor = "EndWeekJob"
                },
                new Data.Job
                {
                    Id = Guid.NewGuid().ToString("D"),
                    Name = "job2",
                    UserId = _user.Id,
                    Trigger = DateTime.UtcNow.AddMinutes(1),
                    Processor = "StartWeekJob"
                },
                new Data.Job
                {
                    Id = Guid.NewGuid().ToString("D"),
                    Name = "job3",
                    UserId = _task.Id,
                    Trigger = DateTime.UtcNow.AddMinutes(2),
                    Processor = "StartWeekJob"
                },
                new Data.Job
                {
                    Id = Guid.NewGuid().ToString("D"),
                    Name = "job4",
                    UserId = _task.Id,
                    Trigger = DateTime.UtcNow.AddMinutes(3),
                    Processor = "EndWeekJob"
                }
            };
            Mocked<IDataReader>().Setup(r => r.GetAll<Data.Job>()).Returns(jobs);

            var model = _mapper.Map<Data.User, Model.User>(_user);

            model.ShouldSatisfyAllConditions(
                () => model.PlanningEndTime.ShouldBe(jobs[1].Trigger),
                () => model.ActiveStartTime.ShouldBe(jobs[0].Trigger));
        }

        [Test]
        public void RavenUserLookupCannotFindStartJob()
        {
            var jobs = new List<Data.Job>
            {
                new Data.Job
                {
                    Id = Guid.NewGuid().ToString("D"),
                    Name = "job1",
                    UserId = _user.Id,
                    Trigger = DateTime.UtcNow,
                    Processor = "EndWeekJob"
                },
                new Data.Job
                {
                    Id = Guid.NewGuid().ToString("D"),
                    Name = "job3",
                    UserId = _task.Id,
                    Trigger = DateTime.UtcNow.AddMinutes(2),
                    Processor = "StartWeekJob"
                },
                new Data.Job
                {
                    Id = Guid.NewGuid().ToString("D"),
                    Name = "job4",
                    UserId = _task.Id,
                    Trigger = DateTime.UtcNow.AddMinutes(3),
                    Processor = "EndWeekJob"
                }
            };
            Mocked<IDataReader>().Setup(r => r.GetAll<Data.Job>()).Returns(jobs);

            var model = _mapper.Map<Data.User, Model.User>(_user);

            model.ShouldSatisfyAllConditions(
                () => model.PlanningEndTime.ShouldBeNull(),
                () => model.ActiveStartTime.ShouldBe(jobs[0].Trigger));
        }

        [Test]
        public void RavenUserLookupCannotFindEndJob()
        {
            var jobs = new List<Data.Job>
            {
                new Data.Job
                {
                    Id = Guid.NewGuid().ToString("D"),
                    Name = "job2",
                    UserId = _user.Id,
                    Trigger = DateTime.UtcNow.AddMinutes(1),
                    Processor = "StartWeekJob"
                },
                new Data.Job
                {
                    Id = Guid.NewGuid().ToString("D"),
                    Name = "job3",
                    UserId = _task.Id,
                    Trigger = DateTime.UtcNow.AddMinutes(2),
                    Processor = "StartWeekJob"
                },
                new Data.Job
                {
                    Id = Guid.NewGuid().ToString("D"),
                    Name = "job4",
                    UserId = _task.Id,
                    Trigger = DateTime.UtcNow.AddMinutes(3),
                    Processor = "EndWeekJob"
                }
            };
            Mocked<IDataReader>().Setup(r => r.GetAll<Data.Job>()).Returns(jobs);

            var model = _mapper.Map<Data.User, Model.User>(_user);

            model.ShouldSatisfyAllConditions(
                () => model.PlanningEndTime.ShouldBe(jobs[0].Trigger),
                () => model.ActiveStartTime.ShouldBeNull());
        }

        [Test]
        public void RavenUserLookupCannotFindJobs()
        {
            var jobs = new List<Data.Job>
            {
                new Data.Job
                {
                    Id = Guid.NewGuid().ToString("D"),
                    Name = "job3",
                    UserId = _task.Id,
                    Trigger = DateTime.UtcNow.AddMinutes(2),
                    Processor = "StartWeekJob"
                },
                new Data.Job
                {
                    Id = Guid.NewGuid().ToString("D"),
                    Name = "job4",
                    UserId = _task.Id,
                    Trigger = DateTime.UtcNow.AddMinutes(3),
                    Processor = "EndWeekJob"
                }
            };
            Mocked<IDataReader>().Setup(r => r.GetAll<Data.Job>()).Returns(jobs);

            var model = _mapper.Map<Data.User, Model.User>(_user);

            model.ShouldSatisfyAllConditions(
                () => model.PlanningEndTime.ShouldBeNull(),
                () => model.ActiveStartTime.ShouldBeNull());
        }
    }
}