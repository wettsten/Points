using System;
using System.Collections.Generic;
using AutoMapper;
using AutoMoq.Helpers;
using NUnit.Framework;
using Points.Common.AutoMapper;
using Points.DataAccess.Readers;
using Shouldly;

namespace Points.Api.UnitTests.Common.AutoMapper
{
    [TestFixture]
    public class OtherMapperTests : AutoMoqTestFixture<MappingProfile>
    {
        private IMapper _mapper;

        private Model.Duration m_duration;
        private Model.Frequency m_frequency;
        private Model.Category m_cat;
        private Model.Task m_task;
        private Model.PlanningTask m_pTask;
        private Model.ActiveTask m_aTask;

        private Data.Duration d_duration;
        private Data.Frequency d_frequency;
        private Data.Category d_cat;
        private Data.Task d_task;
        private Data.PlanningTask d_pTask;
        private Data.ActiveTask d_aTask;

        [OneTimeSetUp]
        [TestFixtureSetUp]
        public void TestDataSetup()
        {
            _mapper = new MapperConfiguration(mapper => mapper.AddProfile(Subject)).CreateMapper();
            
            SetupData();
            SetupModel();
        }

        private void SetupData()
        {
            d_duration = new Data.Duration
            {
                Type = Data.DurationType.AtLeast,
                Value = 1,
                Unit = Data.DurationUnit.Hours
            };
            d_frequency = new Data.Frequency
            {
                Type = Data.FrequencyType.AtLeast,
                Value = 1,
                Unit = Data.FrequencyUnit.Times
            };
            d_cat = new Data.Category
            {
                Id = Guido.New(),
                Name = "catName"
            };
            d_task = new Data.Task
            {
                Id = Guido.New(),
                Name = "taskName",
                CategoryId = d_cat.Id
            };
            d_pTask = new Data.PlanningTask
            {
                Id = Guido.New(),
                Name = "pTaskName",
                TaskId = d_task.Id,
                Duration = d_duration,
                Frequency = d_frequency
            };
            d_aTask = new Data.ActiveTask
            {
                Id = Guido.New(),
                Name = "aTaskName",
                TaskName = d_task.Name,
                CategoryName = d_cat.Name,
                Duration = d_duration,
                Frequency = d_frequency,
                DateStarted = DateTime.UtcNow,
                TimesCompleted = 3
            };

            Mocked<IDataReader>().Setup(r => r.Get<Data.Category>(d_task.CategoryId)).Returns(d_cat);
            Mocked<IDataReader>().Setup(r => r.Get<Data.Task>(d_pTask.TaskId)).Returns(d_task);
        }

        private void SetupModel()
        {
            m_duration = new Model.Duration
            {
                Type = new Model.ModelBase { Id = "AtLeast" },
                Value = 1,
                Unit = new Model.ModelBase { Id = "Hours" }
            };
            m_frequency = new Model.Frequency
            {
                Type = new Model.ModelBase { Id = "AtLeast" },
                Value = 1,
                Unit = new Model.ModelBase { Id = "Times" }
            };
            m_cat = new Model.Category
            {
                Id = Guido.New(),
                Name = "catName"
            };
            m_task = new Model.Task
            {
                Id = Guido.New(),
                Name = "taskName",
                Category = m_cat
            };
            m_pTask = new Model.PlanningTask
            {
                Id = Guido.New(),
                Name = "pTaskName",
                Task = m_task,
                Duration = m_duration,
                Frequency = m_frequency
            };
            m_aTask = new Model.ActiveTask
            {
                Id = Guido.New(),
                Name = "aTaskName",
                TaskName = m_task.Name,
                CategoryName = m_cat.Name,
                Duration = m_duration,
                Frequency = m_frequency,
                DateStarted = DateTime.UtcNow,
                TimesCompleted = 3
            };
        }

        [Test]
        public void DataPlanningTaskToDataActiveTask()
        {
            var data = _mapper.Map<Data.PlanningTask, Data.ActiveTask>(d_pTask);

            data.ShouldSatisfyAllConditions(
                () => data.Id.ShouldBe(d_pTask.Id),
                () => data.Name.ShouldBe(d_pTask.Name),
                () => data.TaskName.ShouldBe(d_task.Name),
                () => data.CategoryName.ShouldBe(d_cat.Name),
                () => data.Duration.Type.ShouldBe(d_duration.Type),
                () => data.Duration.Value.ShouldBe(d_duration.Value),
                () => data.Duration.Unit.ShouldBe(d_duration.Unit),
                () => data.Frequency.Type.ShouldBe(d_frequency.Type),
                () => data.Frequency.Value.ShouldBe(d_frequency.Value),
                () => data.Frequency.Unit.ShouldBe(d_frequency.Unit));
        }

        [Test]
        public void ModelPlanningTaskToModelActiveTask()
        {
            var data = _mapper.Map<Model.PlanningTask, Model.ActiveTask>(m_pTask);

            data.ShouldSatisfyAllConditions(
                () => data.Id.ShouldBe(m_pTask.Id),
                () => data.Name.ShouldBe(m_pTask.Name),
                () => data.TaskName.ShouldBe(m_task.Name),
                () => data.CategoryName.ShouldBe(m_cat.Name),
                () => data.Duration.Type.Id.ShouldBe(m_duration.Type.Id),
                () => data.Duration.Type.Name.ShouldBe(m_duration.Type.Name),
                () => data.Duration.Value.ShouldBe(m_duration.Value),
                () => data.Duration.Unit.Id.ShouldBe(m_duration.Unit.Id),
                () => data.Duration.Unit.Name.ShouldBe(m_duration.Unit.Name),
                () => data.Frequency.Type.Id.ShouldBe(m_frequency.Type.Id),
                () => data.Frequency.Type.Name.ShouldBe(m_frequency.Type.Name),
                () => data.Frequency.Value.ShouldBe(m_frequency.Value),
                () => data.Frequency.Unit.Id.ShouldBe(m_frequency.Unit.Id),
                () => data.Frequency.Unit.Name.ShouldBe(m_frequency.Unit.Name));
        }

        [Test]
        public void DataActiveTaskToDataArchivedTask()
        {
            var data = _mapper.Map<Data.ActiveTask, Data.ArchivedTask>(d_aTask);

            data.ShouldSatisfyAllConditions(
                () => data.Id.ShouldBe(d_aTask.Id),
                () => data.Name.ShouldBe(d_aTask.Name),
                () => data.TaskName.ShouldBe(d_task.Name),
                () => data.CategoryName.ShouldBe(d_cat.Name),
                () => data.Duration.Type.ShouldBe(d_duration.Type),
                () => data.Duration.Value.ShouldBe(d_duration.Value),
                () => data.Duration.Unit.ShouldBe(d_duration.Unit),
                () => data.Frequency.Type.ShouldBe(d_frequency.Type),
                () => data.Frequency.Value.ShouldBe(d_frequency.Value),
                () => data.Frequency.Unit.ShouldBe(d_frequency.Unit),
                () => data.TimesCompleted.ShouldBe(d_aTask.TimesCompleted));
        }

        [Test]
        public void ModelActiveTaskToModelArchivedTask()
        {
            var data = _mapper.Map<Model.ActiveTask, Model.ArchivedTask>(m_aTask);

            data.ShouldSatisfyAllConditions(
                () => data.Id.ShouldBe(m_aTask.Id),
                () => data.Name.ShouldBe(m_aTask.Name),
                () => data.TaskName.ShouldBe(m_task.Name),
                () => data.CategoryName.ShouldBe(m_cat.Name),
                () => data.Duration.Type.Id.ShouldBe(m_duration.Type.Id),
                () => data.Duration.Type.Name.ShouldBe(m_duration.Type.Name),
                () => data.Duration.Value.ShouldBe(m_duration.Value),
                () => data.Duration.Unit.Id.ShouldBe(m_duration.Unit.Id),
                () => data.Duration.Unit.Name.ShouldBe(m_duration.Unit.Name),
                () => data.Frequency.Type.Id.ShouldBe(m_frequency.Type.Id),
                () => data.Frequency.Type.Name.ShouldBe(m_frequency.Type.Name),
                () => data.Frequency.Value.ShouldBe(m_frequency.Value),
                () => data.Frequency.Unit.Id.ShouldBe(m_frequency.Unit.Id),
                () => data.Frequency.Unit.Name.ShouldBe(m_frequency.Unit.Name),
                () => data.TimesCompleted.ShouldBe(m_aTask.TimesCompleted));
        }
    }
}