using System;
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
    public class GetActiveTotalsTests : AutoMoqTestFixture<RequestProcessor>
    {
        private readonly string _userId = Guido.New();
        private List<Data.ActiveTask> _tasks;
        private Dictionary<string, string> _catIds = new Dictionary<string, string>();

        [TestFixtureSetUp]
        public void Setup()
        {
            SetupTasks();
            
            Mocked<IMapFactory>().Setup(r => r.GetDestinationType(typeof(Model.ActiveTask))).Returns(typeof(Data.ActiveTask));
            Mocked<IDataReader>().Setup(r => r.GetAll(typeof (Data.ActiveTask))).Returns(new List<Data.DataBase>(_tasks));
            foreach (var task in _tasks)
            {
                Mocked<IMapFactory>().Setup(r => r.MapToViewObject(It.Is<Data.DataBase>(s => s.Id.Equals(task.Id)))).Returns(MapToModelTask(task));
            }
        }

        private void SetupTasks()
        {
            _tasks = new List<Data.ActiveTask>
            {
                new Data.ActiveTask
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
                    TaskId = Guido.New(),
                    DateStarted = DateTime.UtcNow.AddDays(-4),
                    TimesCompleted = 8
                },
                new Data.ActiveTask
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
                        Type = Data.FrequencyType.AtMost,
                        Value = 4,
                        Unit = Data.FrequencyUnit.Times
                    },
                    TaskId = Guido.New(),
                    DateStarted = DateTime.UtcNow.AddDays(-4),
                    TimesCompleted = 2
                },
                new Data.ActiveTask
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
                    TaskId = Guido.New(),
                    DateStarted = DateTime.UtcNow.AddDays(-4),
                    TimesCompleted = 9
                },
                new Data.ActiveTask
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
                    TaskId = Guido.New(),
                    DateStarted = DateTime.UtcNow.AddDays(-4),
                    TimesCompleted = 6
                }
            };
        }

        private Model.ActiveTask MapToModelTask(Data.ActiveTask task)
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
            return new Model.ActiveTask
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
                BonusPointValue = task.BonusPointValue,
                DateStarted = task.DateStarted,
                TimesCompleted = task.TimesCompleted,
                IsCompleted = task.IsCompleted,
                BonusPoints = task.BonusPoints
            };
        }

        [Test]
        public void GetPlanningTotalsVerifyData()
        {
            var totals = Subject.GetActiveTotals(_userId);

            totals.ShouldSatisfyAllConditions(
                () => totals.IsCompleted.ShouldBeFalse(),
                () => totals.TargetPoints.ShouldBe(4),
                () => totals.TaskPoints.ShouldBe(3),
                () => totals.BonusPoints.ShouldBe(0.825m),
                () => totals.TotalPoints.ShouldBe(3.825m),
                () => totals.Categories.Count().ShouldBe(3),
                () => totals.Categories.ElementAt(0).Name.ShouldBe("cat1"),
                () => totals.Categories.ElementAt(0).IsCompleted.ShouldBeTrue(),
                () => totals.Categories.ElementAt(0).TargetPoints.ShouldBe(2),
                () => totals.Categories.ElementAt(0).TaskPoints.ShouldBe(2),
                () => totals.Categories.ElementAt(0).BonusPoints.ShouldBe(0.7m),
                () => totals.Categories.ElementAt(0).TotalPoints.ShouldBe(2.7m),
                () => totals.Categories.ElementAt(0).Tasks.Count().ShouldBe(2),
                () => totals.Categories.ElementAt(0).Tasks.ElementAt(0).Name.ShouldBe("task1"),
                () => totals.Categories.ElementAt(0).Tasks.ElementAt(0).IsCompleted.ShouldBeTrue(),
                () => totals.Categories.ElementAt(0).Tasks.ElementAt(0).TargetPoints.ShouldBe(1),
                () => totals.Categories.ElementAt(0).Tasks.ElementAt(0).TaskPoints.ShouldBe(1),
                () => totals.Categories.ElementAt(0).Tasks.ElementAt(0).BonusPoints.ShouldBe(0.2m),
                () => totals.Categories.ElementAt(0).Tasks.ElementAt(0).TotalPoints.ShouldBe(1.2m),
                () => totals.Categories.ElementAt(0).Tasks.ElementAt(1).Name.ShouldBe("task2"),
                () => totals.Categories.ElementAt(0).Tasks.ElementAt(1).IsCompleted.ShouldBeTrue(),
                () => totals.Categories.ElementAt(0).Tasks.ElementAt(1).TargetPoints.ShouldBe(1),
                () => totals.Categories.ElementAt(0).Tasks.ElementAt(1).TaskPoints.ShouldBe(1),
                () => totals.Categories.ElementAt(0).Tasks.ElementAt(1).BonusPoints.ShouldBe(0.5m),
                () => totals.Categories.ElementAt(0).Tasks.ElementAt(1).TotalPoints.ShouldBe(1.5m),
                () => totals.Categories.ElementAt(1).Name.ShouldBe("cat2"),
                () => totals.Categories.ElementAt(1).IsCompleted.ShouldBeTrue(),
                () => totals.Categories.ElementAt(1).TargetPoints.ShouldBe(1),
                () => totals.Categories.ElementAt(1).TaskPoints.ShouldBe(1),
                () => totals.Categories.ElementAt(1).BonusPoints.ShouldBe(0.125m),
                () => totals.Categories.ElementAt(1).TotalPoints.ShouldBe(1.125m),
                () => totals.Categories.ElementAt(1).Tasks.Count().ShouldBe(1),
                () => totals.Categories.ElementAt(1).Tasks.ElementAt(0).Name.ShouldBe("task4"),
                () => totals.Categories.ElementAt(1).Tasks.ElementAt(0).IsCompleted.ShouldBeTrue(),
                () => totals.Categories.ElementAt(1).Tasks.ElementAt(0).TargetPoints.ShouldBe(1),
                () => totals.Categories.ElementAt(1).Tasks.ElementAt(0).TaskPoints.ShouldBe(1),
                () => totals.Categories.ElementAt(1).Tasks.ElementAt(0).BonusPoints.ShouldBe(0.125m),
                () => totals.Categories.ElementAt(1).Tasks.ElementAt(0).TotalPoints.ShouldBe(1.125m),
                () => totals.Categories.ElementAt(2).Name.ShouldBe("cat3"),
                () => totals.Categories.ElementAt(2).IsCompleted.ShouldBeFalse(),
                () => totals.Categories.ElementAt(2).TargetPoints.ShouldBe(1),
                () => totals.Categories.ElementAt(2).TaskPoints.ShouldBe(0),
                () => totals.Categories.ElementAt(2).BonusPoints.ShouldBe(0.0m),
                () => totals.Categories.ElementAt(2).TotalPoints.ShouldBe(0.0m),
                () => totals.Categories.ElementAt(2).Tasks.Count().ShouldBe(1),
                () => totals.Categories.ElementAt(2).Tasks.ElementAt(0).Name.ShouldBe("task3"),
                () => totals.Categories.ElementAt(2).Tasks.ElementAt(0).IsCompleted.ShouldBeFalse(),
                () => totals.Categories.ElementAt(2).Tasks.ElementAt(0).TargetPoints.ShouldBe(1),
                () => totals.Categories.ElementAt(2).Tasks.ElementAt(0).TaskPoints.ShouldBe(0),
                () => totals.Categories.ElementAt(2).Tasks.ElementAt(0).BonusPoints.ShouldBe(0.0m),
                () => totals.Categories.ElementAt(2).Tasks.ElementAt(0).TotalPoints.ShouldBe(0.0m));
        }
    }
}