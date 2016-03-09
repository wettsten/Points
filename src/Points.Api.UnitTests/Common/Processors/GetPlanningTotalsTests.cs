using System.Collections.Generic;
using System.Linq;
using AutoMoq.Helpers;
using Moq;
using NUnit.Framework;
using Points.Common.Extensions;
using Points.Common.Factories;
using Points.Common.Processors;
using Points.DataAccess.Readers;
using Shouldly;

namespace Points.Api.UnitTests.Common.Processors
{
    [TestFixture]
    public class GetPlanningTotalsTests : AutoMoqTestFixture<RequestProcessor>
    {
        private readonly string _userId = Guido.New();
        private List<Data.PlanningTask> _tasks;
        private Dictionary<string, string> _catIds = new Dictionary<string, string>();

        [TestFixtureSetUp]
        public void Setup()
        {
            SetupTasks();
            
            Mocked<IMapFactory>().Setup(r => r.GetDestinationType(typeof(Model.PlanningTask))).Returns(typeof(Data.PlanningTask));
            Mocked<IDataReader>().Setup(r => r.GetAll(typeof (Data.PlanningTask))).Returns(new List<Data.DataBase>(_tasks));
            foreach (var task in _tasks)
            {
                Mocked<IMapFactory>().Setup(r => r.MapToViewObject(It.Is<Data.DataBase>(s => s.Id.Equals(task.Id)))).Returns(MapToModelTask(task));
            }
        }

        private void SetupTasks()
        {
            _tasks = new List<Data.PlanningTask>
            {
                new Data.PlanningTask
                {
                    Id = Guido.New(),
                    Name = "task3|cat3",
                    UserId = _userId,
                    Duration = new Data.Duration
                    {
                        Type = Data.DurationType.AtLeast,
                        Value = 30,
                        Unit = Data.DurationUnit.Minutes
                    },
                    Frequency = new Data.Frequency
                    {
                        Type = Data.FrequencyType.AtLeast,
                        Value = 10,
                        Unit = Data.FrequencyUnit.Times
                    },
                    TaskId = Guido.New()
                },
                new Data.PlanningTask
                {
                    Id = Guido.New(),
                    Name = "task2|cat1",
                    UserId = _userId,
                    Duration = new Data.Duration
                    {
                        Type = Data.DurationType.AtLeast,
                        Value = 30,
                        Unit = Data.DurationUnit.Minutes
                    },
                    Frequency = new Data.Frequency
                    {
                        Type = Data.FrequencyType.AtLeast,
                        Value = 4,
                        Unit = Data.FrequencyUnit.Times
                    },
                    TaskId = Guido.New()
                },
                new Data.PlanningTask
                {
                    Id = Guido.New(),
                    Name = "task4|cat2",
                    UserId = _userId,
                    Duration = new Data.Duration
                    {
                        Type = Data.DurationType.AtLeast,
                        Value = 30,
                        Unit = Data.DurationUnit.Minutes
                    },
                    Frequency = new Data.Frequency
                    {
                        Type = Data.FrequencyType.AtLeast,
                        Value = 8,
                        Unit = Data.FrequencyUnit.Times
                    },
                    TaskId = Guido.New()
                },
                new Data.PlanningTask
                {
                    Id = Guido.New(),
                    Name = "task1|cat1",
                    UserId = _userId,
                    Duration = new Data.Duration
                    {
                        Type = Data.DurationType.AtLeast,
                        Value = 30,
                        Unit = Data.DurationUnit.Minutes
                    },
                    Frequency = new Data.Frequency
                    {
                        Type = Data.FrequencyType.AtLeast,
                        Value = 5,
                        Unit = Data.FrequencyUnit.Times
                    },
                    TaskId = Guido.New()
                }
            };
        }

        private Model.PlanningTask MapToModelTask(Data.PlanningTask task)
        {
            string catId = Guido.New();
            if (!_catIds.ContainsKey(task.Name.Split('|')[1]))
            {
                _catIds.Add(task.Name.Split('|')[1], catId);
            }
            else
            {
                catId = _catIds[task.Name.Split('|')[1]];
            }
            return new Model.PlanningTask
            {
                Id = task.Id,
                Name = task.Name.Split('|')[0],
                Duration = new Model.Duration
                {
                    Type = new Model.ModelBase
                    {
                        Id = task.Duration.Type.ToString(),
                        Name = task.Duration.Type.Spacify()
                    },
                    Value = task.Duration.Value,
                    Unit = new Model.ModelBase
                    {
                        Id = task.Duration.Unit.ToString(),
                        Name = task.Duration.Unit.Spacify()
                    }
                },
                Frequency = new Model.Frequency
                {
                    Type = new Model.ModelBase
                    {
                        Id = task.Frequency.Type.ToString(),
                        Name = task.Frequency.Type.Spacify()
                    },
                    Value = task.Frequency.Value,
                    Unit = new Model.ModelBase
                    {
                        Id = task.Frequency.Unit.ToString(),
                        Name = task.Frequency.Unit.Spacify()
                    }
                },
                Task = new Model.Task
                {
                    Id = task.TaskId,
                    Name = task.Name.Split('|')[0],
                    Category = new Model.Category
                    {
                        Id = catId,
                        Name = task.Name.Split('|')[1]
                    }
                },
                BonusPointValue = task.BonusPointValue
            };
        }

        [Test]
        public void GetPlanningTotalsVerifyData()
        {
            var totals = Subject.GetPlanningTotals(_userId);

            totals.ShouldSatisfyAllConditions(
                () => totals.Points.ShouldBe(4),
                () => totals.Categories.Count().ShouldBe(3),
                () => totals.Categories.ElementAt(0).Name.ShouldBe("cat1"),
                () => totals.Categories.ElementAt(0).Points.ShouldBe(2),
                () => totals.Categories.ElementAt(0).Tasks.Count().ShouldBe(2),
                () => totals.Categories.ElementAt(0).Tasks.ElementAt(0).Name.ShouldBe("task1"),
                () => totals.Categories.ElementAt(0).Tasks.ElementAt(0).Points.ShouldBe(1),
                () => totals.Categories.ElementAt(0).Tasks.ElementAt(0).BonusPointValue.ShouldBe(0.2m),
                () => totals.Categories.ElementAt(0).Tasks.ElementAt(1).Name.ShouldBe("task2"),
                () => totals.Categories.ElementAt(0).Tasks.ElementAt(1).Points.ShouldBe(1),
                () => totals.Categories.ElementAt(0).Tasks.ElementAt(1).BonusPointValue.ShouldBe(0.25m),
                () => totals.Categories.ElementAt(1).Name.ShouldBe("cat2"),
                () => totals.Categories.ElementAt(1).Points.ShouldBe(1),
                () => totals.Categories.ElementAt(1).Tasks.Count().ShouldBe(1),
                () => totals.Categories.ElementAt(1).Tasks.ElementAt(0).Name.ShouldBe("task4"),
                () => totals.Categories.ElementAt(1).Tasks.ElementAt(0).Points.ShouldBe(1),
                () => totals.Categories.ElementAt(1).Tasks.ElementAt(0).BonusPointValue.ShouldBe(0.125m),
                () => totals.Categories.ElementAt(2).Name.ShouldBe("cat3"),
                () => totals.Categories.ElementAt(2).Points.ShouldBe(1),
                () => totals.Categories.ElementAt(2).Tasks.Count().ShouldBe(1),
                () => totals.Categories.ElementAt(2).Tasks.ElementAt(0).Name.ShouldBe("task3"),
                () => totals.Categories.ElementAt(2).Tasks.ElementAt(0).Points.ShouldBe(1),
                () => totals.Categories.ElementAt(2).Tasks.ElementAt(0).BonusPointValue.ShouldBe(0.1m));
        }
    }
}