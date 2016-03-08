using System;
using System.Collections.Generic;
using System.Linq;
using Points.Common.Extensions;
using Points.Common.Factories;
using Points.Model;
using Points.DataAccess.Readers;
using Points.DataAccess.Writers;

namespace Points.Common.Processors
{
    public class RequestProcessor : IRequestProcessor
    {
        private readonly IDataReader _dataReader;
        private readonly IDataWriter _dataWriter;
        private readonly IObjectValidatorFactory _objectValidatorFactory;
        private readonly IMapFactory _mapFactory;

        public RequestProcessor(IDataReader dataReader, IDataWriter dataWriter, IObjectValidatorFactory objectValidatorFactory, IMapFactory mapFactory)
        {
            _dataReader = dataReader;
            _dataWriter = dataWriter;
            _objectValidatorFactory = objectValidatorFactory;
            _mapFactory = mapFactory;
        }

        public void AddData<TView>(TView data, string userId) where TView : ViewObject
        {
            // map to RavenObject
            var ravenObj = _mapFactory.MapToRavenObject(data);
            ravenObj.UserId = userId;
            var validator = _objectValidatorFactory.Get(ravenObj.GetType());
            validator?.ValidateAdd(ravenObj);
            _dataWriter.Add(ravenObj);
        }

        public void EditData<TView>(TView data, string userId) where TView : ViewObject
        {
            // map to RavenObject
            var ravenObj = _mapFactory.MapToRavenObject(data);
            ravenObj.UserId = userId;
            var validator = _objectValidatorFactory.Get(ravenObj.GetType());
            validator?.ValidateEdit(ravenObj);
            _dataWriter.Edit(ravenObj);
        }

        public void DeleteData<TView>(TView data, string userId) where TView : ViewObject
        {
            // map to RavenObject
            var ravenObj = _mapFactory.MapToRavenObject(data);
            ravenObj.UserId = userId;
            var validator = _objectValidatorFactory.Get(ravenObj.GetType());
            validator?.ValidateDelete(ravenObj);
            _dataWriter.Delete(ravenObj);
        }

        public IList<TView> GetListForUser<TView>(string userId) where TView : ViewObject
        {
            var ravenType = _mapFactory.GetDestinationType(typeof (TView));
            var objs = _dataReader
                .GetAll(ravenType)
                .Where(i => i.UserId.Equals(userId, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
            return objs.Select(i => (TView)_mapFactory.MapToViewObject(i)).ToList();
        }

        public IEnumerable<ViewObject> GetEnums(string enumType)
        {
            var eType = Type.GetType("Points.Data." + enumType + ", Points.Data");
            if (eType != null)
            {
                return Enum.GetValues(eType).Cast<object>().Select(i => new ViewObject
                {
                    Id = i.ToString(),
                    Name = i.Spacify()
                });
            }
            return new List<ViewObject>();
        }

        public dynamic GetPlanningTotals(string userId)
        {
            var tasks = GetListForUser<PlanningTask>(userId);
            var cats = tasks
                .GroupBy(i => i.Task.Category.Id, task => task)
                .Select(i => new
                {
                    Id = i.Key,
                    Name = i.First().Task.Category.Name,
                    Points = i.Count(),
                    Tasks = i.OrderBy(j => j.Name)
                })
                .OrderBy(i => i.Name);
            return new
            {
                Points = cats.Sum(i => i.Points),
                Categories = cats
            };
        }

        public dynamic GetActiveTotals(string userId)
        {
            var tasks = GetListForUser<ActiveTask>(userId);
            var cats = tasks
                .GroupBy(i => i.Task.Category.Id, task => task)
                .Select(i => new
                {
                    Id = i.Key,
                    Name = i.First().Task.Category.Name,
                    IsCompleted = i.All(j => j.IsCompleted),
                    TargetPoints = i.Count(),
                    TaskPoints = i.Count(j => j.IsCompleted),
                    BonusPoints = i.Sum(j => j.BonusPoints),
                    TotalPoints = i.Count(j => j.IsCompleted) + i.Sum(j => j.BonusPoints),
                    Tasks = i.Select(j => new
                    {
                        j.Id,
                        j.Name,
                        j.IsCompleted,
                        TargetPoints = 1,
                        TaskPoints = j.IsCompleted ? 1 : 0,
                        j.BonusPoints,
                        TotalPoints = (j.IsCompleted ? 1 : 0) + j.BonusPoints
                    }).OrderBy(j => j.Name)
                })
                .OrderBy(i => i.Name);
            return new
            {
                IsCompleted = cats.All(i => i.IsCompleted),
                TargetPoints = cats.Sum(i => i.TargetPoints),
                TaskPoints = cats.Sum(i => i.TaskPoints),
                BonusPoints = cats.Sum(i => i.BonusPoints),
                TotalPoints = cats.Sum(i => i.TotalPoints),
                Categories = cats
            };
        }
    }
}