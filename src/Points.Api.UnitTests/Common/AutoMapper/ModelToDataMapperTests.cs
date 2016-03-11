using System;
using AutoMapper;
using AutoMoq.Helpers;
using NUnit.Framework;
using Points.Common.AutoMapper;
using Shouldly;

namespace Points.Api.UnitTests.Common.AutoMapper
{
    [TestFixture]
    public class ModelToDataMapperTests : AutoMoqTestFixture<MappingProfile>
    {
        private IMapper _mapper;

        private Model.Duration _duration;
        private Model.Frequency _frequency;
        private Model.Category _cat;
        private Model.Task _task;
        private Model.PlanningTask _pTask;
        private Model.ActiveTask _aTask;
        private Model.ArchivedTask _arcTask;
        private Model.Job _job;
        private Model.User _user;

        [OneTimeSetUp]
        [TestFixtureSetUp]
        public void TestDataSetup()
        {
            _mapper = new MapperConfiguration(mapper => mapper.AddProfile(Subject)).CreateMapper();
            
            SetupData();
        }

        private void SetupData()
        {
            _duration = new Model.Duration
            {
                Type = new Model.ModelBase { Id = "AtLeast" },
                Value = 1,
                Unit = new Model.ModelBase { Id = "Hours" }
            };
            _frequency = new Model.Frequency
            {
                Type = new Model.ModelBase { Id = "AtLeast" },
                Value = 1,
                Unit = new Model.ModelBase { Id = "Times" }
            };
            _cat = new Model.Category
            {
                Id = Guido.New(),
                Name = "catName"
            };
            _task = new Model.Task
            {
                Id = Guido.New(),
                Name = "taskName",
                Category = _cat
            };
            _pTask = new Model.PlanningTask
            {
                Id = Guido.New(),
                Name = "pTaskName",
                Task = _task,
                Duration = _duration,
                Frequency = _frequency
            };
            _aTask = new Model.ActiveTask
            {
                Id = Guido.New(),
                Name = "aTaskName",
                TaskName = _task.Name,
                CategoryName = _cat.Name,
                Duration = _duration,
                Frequency = _frequency,
                DateStarted = DateTime.UtcNow,
                TimesCompleted = 3
            };
            _arcTask = new Model.ArchivedTask
            {
                Id = Guido.New(),
                Name = "arcTaskName",
                TaskName = _task.Name,
                CategoryName = _cat.Name,
                Duration = _duration,
                Frequency = _frequency,
                DateStarted = DateTime.UtcNow,
                TimesCompleted = 3,
                DateEnded = DateTime.UtcNow.AddMinutes(1)
            };
            _job = new Model.Job
            {
                Id = Guido.New(),
                Name = "jobName",
                Processor = "jobProcessor",
                Trigger = DateTime.UtcNow
            };
            _user = new Model.User
            {
                Id = Guido.New(),
                Name = "userName",
                Email = "a@a",
                TargetPoints = 10,
                WeekSummaryEmail = true,
                WeekStartDay = DayOfWeek.Friday,
                WeekStartHour = Model.SimpleInt.FromId(12),
                NotifyWeekStarting = Model.SimpleInt.FromId(2),
                NotifyWeekEnding = Model.SimpleInt.FromId(3),
                EnableAdvancedFeatures = true,
                ActiveTargetPoints = 15,
                CategoryBonus = 1,
                TaskMultiplier = 2,
                BonusPointMultiplier = 3,
                DurationBonusPointsPerHour = 2
            };
        }

        [Test]
        public void ViewObjectToRavenObject()
        {
            var obj = new Model.ModelBase()
            {
                Id = Guido.New(),
                Name = "name"
            };

            var data = _mapper.Map<Model.ModelBase, Data.DataBase>(obj);

            data.ShouldSatisfyAllConditions(
                () => data.Id.ShouldBe(obj.Id),
                () => data.Name.ShouldBe(obj.Name));
        }

        [Test]
        public void RavenDurationToViewDuration()
        {
            var data = _mapper.Map<Model.Duration, Data.Duration>(_duration);

            data.ShouldSatisfyAllConditions(
                () => data.Type.ToString().ShouldBe(_duration.Type.Id),
                () => data.Value.ShouldBe(_duration.Value),
                () => data.Unit.ToString().ShouldBe(_duration.Unit.Id));
        }

        [Test]
        public void RavenFrequencyToViewFrequency()
        {
            var data = _mapper.Map<Model.Frequency, Data.Frequency>(_frequency);

            data.ShouldSatisfyAllConditions(
                () => data.Type.ToString().ShouldBe(_frequency.Type.Id),
                () => data.Value.ShouldBe(_frequency.Value),
                () => data.Unit.ToString().ShouldBe(_frequency.Unit.Id));
        }

        [Test]
        public void RavenCategoryToViewCategory()
        {
            var data = _mapper.Map<Model.Category, Data.Category>(_cat);

            data.ShouldSatisfyAllConditions(
                () => data.Id.ShouldBe(_cat.Id),
                () => data.Name.ShouldBe(_cat.Name));
        }

        [Test]
        public void RavenTaskToViewTask()
        {
            var data = _mapper.Map<Model.Task, Data.Task>(_task);

            data.ShouldSatisfyAllConditions(
                () => data.Id.ShouldBe(_task.Id),
                () => data.Name.ShouldBe(_task.Name),
                () => data.CategoryId.ShouldBe(_cat.Id));
        }

        [Test]
        public void RavenPlanningTaskToViewPlanningTask()
        {
            var data = _mapper.Map<Model.PlanningTask, Data.PlanningTask>(_pTask);

            data.ShouldSatisfyAllConditions(
                () => data.Id.ShouldBe(_pTask.Id),
                () => data.Name.ShouldBe(_pTask.Name),
                () => data.TaskId.ShouldBe(_task.Id),
                () => data.Duration.Type.ToString().ShouldBe(_duration.Type.Id),
                () => data.Duration.Value.ShouldBe(_duration.Value),
                () => data.Duration.Unit.ToString().ShouldBe(_duration.Unit.Id),
                () => data.Frequency.Type.ToString().ShouldBe(_frequency.Type.Id),
                () => data.Frequency.Value.ShouldBe(_frequency.Value),
                () => data.Frequency.Unit.ToString().ShouldBe(_frequency.Unit.Id));
        }

        [Test]
        public void RavenActiveTaskToViewActiveTask()
        {
            var data = _mapper.Map<Model.ActiveTask, Data.ActiveTask>(_aTask);

            data.ShouldSatisfyAllConditions(
                () => data.Id.ShouldBe(_aTask.Id),
                () => data.Name.ShouldBe(_aTask.Name),
                () => data.TaskName.ShouldBe(_task.Name),
                () => data.CategoryName.ShouldBe(_cat.Name),
                () => data.DateStarted.ShouldBe(_aTask.DateStarted),
                () => data.TimesCompleted.ShouldBe(_aTask.TimesCompleted),
                () => data.Duration.Type.ToString().ShouldBe(_duration.Type.Id),
                () => data.Duration.Value.ShouldBe(_duration.Value),
                () => data.Duration.Unit.ToString().ShouldBe(_duration.Unit.Id),
                () => data.Frequency.Type.ToString().ShouldBe(_frequency.Type.Id),
                () => data.Frequency.Value.ShouldBe(_frequency.Value),
                () => data.Frequency.Unit.ToString().ShouldBe(_frequency.Unit.Id));
        }

        [Test]
        public void RavenArchivedTaskToViewArchivedTask()
        {
            var data = _mapper.Map<Model.ArchivedTask, Data.ArchivedTask>(_arcTask);

            data.ShouldSatisfyAllConditions(
                () => data.Id.ShouldBe(_arcTask.Id),
                () => data.Name.ShouldBe(_arcTask.Name),
                () => data.TaskName.ShouldBe(_task.Name),
                () => data.CategoryName.ShouldBe(_cat.Name),
                () => data.DateStarted.ShouldBe(_arcTask.DateStarted),
                () => data.TimesCompleted.ShouldBe(_arcTask.TimesCompleted),
                () => data.DateEnded.ShouldBe(_arcTask.DateEnded),
                () => data.Duration.Type.ToString().ShouldBe(_duration.Type.Id),
                () => data.Duration.Value.ShouldBe(_duration.Value),
                () => data.Duration.Unit.ToString().ShouldBe(_duration.Unit.Id),
                () => data.Frequency.Type.ToString().ShouldBe(_frequency.Type.Id),
                () => data.Frequency.Value.ShouldBe(_frequency.Value),
                () => data.Frequency.Unit.ToString().ShouldBe(_frequency.Unit.Id));
        }

        [Test]
        public void RavenJobToViewJob()
        {
            var data = _mapper.Map<Model.Job, Data.Job>(_job);

            data.ShouldSatisfyAllConditions(
                () => data.Id.ShouldBe(_job.Id),
                () => data.Name.ShouldBe(_job.Name),
                () => data.Processor.ShouldBe(_job.Processor),
                () => data.Trigger.ShouldBe(_job.Trigger));
        }

        [Test]
        public void RavenUserToViewUser()
        {
            var data = _mapper.Map<Model.User, Data.User>(_user);

            data.ShouldSatisfyAllConditions(
                () => data.Id.ShouldBe(_user.Id),
                () => data.Name.ShouldBe(_user.Name),
                () => data.Email.ShouldBe(_user.Email),
                () => data.TargetPoints.ShouldBe(_user.TargetPoints),
                () => data.WeekSummaryEmail.ShouldBe(_user.WeekSummaryEmail),
                () => data.WeekStartDay.ShouldBe(_user.WeekStartDay),
                () => data.WeekStartHour.ShouldBe(_user.WeekStartHour.Id),
                () => data.NotifyWeekStarting.ShouldBe(_user.NotifyWeekStarting.Id),
                () => data.NotifyWeekEnding.ShouldBe(_user.NotifyWeekEnding.Id),
                () => data.EnableAdvancedFeatures.ShouldBe(_user.EnableAdvancedFeatures),
                () => data.ActiveTargetPoints.ShouldBe(_user.ActiveTargetPoints),
                () => data.CategoryBonus.ShouldBe(_user.CategoryBonus),
                () => data.TaskMultiplier.ShouldBe(_user.TaskMultiplier),
                () => data.BonusPointMultiplier.ShouldBe(_user.BonusPointMultiplier),
                () => data.DurationBonusPointsPerHour.ShouldBe(_user.DurationBonusPointsPerHour));
        }
    }
}