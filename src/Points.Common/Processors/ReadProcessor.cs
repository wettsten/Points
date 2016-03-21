using System;
using System.Collections.Generic;
using System.Linq;
using Points.Common.Extensions;
using Points.Common.Factories;
using Points.Model;
using Points.DataAccess.Readers;

namespace Points.Common.Processors
{
    public class ReadProcessor : IReadProcessor
    {
        private readonly IDataReader _dataReader;
        private readonly IMapFactory _mapFactory;

        public ReadProcessor(IDataReader dataReader, IMapFactory mapFactory)
        {
            _dataReader = dataReader;
            _mapFactory = mapFactory;
        }

        public IEnumerable<TView> GetListForUser<TView>(string userId) where TView : ModelBase
        {
            var ravenType = _mapFactory.GetDestinationType(typeof (TView));
            var objs = _dataReader
                .GetAll(ravenType)
                .Where(i => i.UserId.Equals(userId, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
            return objs.Select(i => (TView)_mapFactory.MapToViewObject(i)).ToList();
        }

        public IEnumerable<ModelBase> GetEnums(string enumType)
        {
            var eType = Type.GetType("Points.Data." + enumType + ", Points.Data");
            if (eType != null)
            {
                return Enum.GetValues(eType).Cast<object>().Select(i => new ModelBase
                {
                    Id = i.ToString(),
                    Name = i.Spacify()
                });
            }
            return new List<ModelBase>();
        }

        public PlanningTotal GetPlanningTotals(string userId)
        {
            var tasks = GetListForUser<PlanningTask>(userId);
            var cats = tasks
                .GroupBy(i => i.Task.Category.Id, task => task)
                .Select(i => new PlanningTotalCategory
                {
                    Id = i.Key,
                    Name = i.First().Task.Category.Name,
                    Points = i.Count(),
                    Tasks = i.Select(j => new PlanningTotalTask
                        {
                            Id = j.Id,
                            Name = j.Name,
                            Points = 1,
                            BonusPointValue = j.BonusPointValue
                        }).OrderBy(j => j.Name)
                })
                .OrderBy(i => i.Name);
            return new PlanningTotal
            {
                Points = cats.Sum(i => i.Points),
                Categories = cats
            };
        }

        public ActiveTotal GetActiveTotals(string userId)
        {
            var tasks = GetListForUser<ActiveTask>(userId);
            var cats = tasks
                .GroupBy(i => i.CategoryName, task => task)
                .Select(i => new ActiveTotalCategory
                {
                    Name = i.Key,
                    IsCompleted = i.All(j => j.IsCompleted),
                    TargetPoints = i.Count(),
                    TaskPoints = i.Count(j => j.IsCompleted),
                    BonusPoints = i.Sum(j => j.BonusPoints),
                    TotalPoints = i.Count(j => j.IsCompleted) + i.Sum(j => j.BonusPoints),
                    Tasks = i.Select(j => new ActiveTotalTask
                    {
                        Name = j.Name,
                        IsCompleted = j.IsCompleted,
                        TargetPoints = 1,
                        TaskPoints = j.IsCompleted ? 1 : 0,
                        BonusPoints = j.BonusPoints,
                        TotalPoints = (j.IsCompleted ? 1 : 0) + j.BonusPoints
                    }).OrderBy(j => j.Name)
                })
                .OrderBy(i => i.Name);
            return new ActiveTotal
            {
                IsCompleted = cats.All(i => i.IsCompleted),
                TargetPoints = cats.Sum(i => i.TargetPoints),
                TaskPoints = cats.Sum(i => i.TaskPoints),
                BonusPoints = cats.Sum(i => i.BonusPoints),
                TotalPoints = cats.Sum(i => i.TotalPoints),
                Categories = cats
            };
        }

        public IEnumerable<AvailableCategory> GetAvailableTasks(string userId)
        {
            var tasks = GetListForUser<Task>(userId);
            var inUseTasks = GetListForUser<PlanningTask>(userId)
                    .Select(i => i.Task.Id);
            var cats = tasks
                .Where(i => !inUseTasks.Contains(i.Id))
                .GroupBy(i => i.Category.Id, task => task)
                .Select(i => new AvailableCategory
                {
                    Id = i.Key,
                    Name = i.First().Category.Name,
                    Tasks = i.OrderBy(j => j.Name)
                })
                .OrderBy(i => i.Name);
            return cats;
        }

        public int GetDocumentCount()
        {
            return _dataReader.GetDocumentCount();
        }
    }
}