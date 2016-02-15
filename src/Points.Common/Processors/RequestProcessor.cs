using System;
using System.Collections.Generic;
using System.Linq;
using Points.Common.EnumExtensions;
using Points.Common.Factories;
using Points.Model;
using Points.DataAccess.Readers;
using Points.DataAccess.Writers;
using StructureMap;

namespace Points.Common.Processors
{
    public class RequestProcessor : IRequestProcessor
    {
        private readonly IDataReader _dataReader;
        private readonly IDataWriter _dataWriter;
        private readonly IObjectValidatorFactory _objectValidatorFactory;
        private readonly IContainer _container;
        private readonly IMapFactory _mapFactory;

        public RequestProcessor(IDataReader dataReader, IDataWriter dataWriter, IObjectValidatorFactory objectValidatorFactory, IContainer container, IMapFactory mapFactory)
        {
            _dataReader = dataReader;
            _dataWriter = dataWriter;
            _objectValidatorFactory = objectValidatorFactory;
            _container = container;
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

        public IList<object> GetEnums(string enumType)
        {
            var output = new List<object>();
            var eType = Type.GetType("Points.Data." + enumType + ", Points.Data");
            if (eType != null)
            {
                foreach (var item in Enum.GetValues(eType))
                {
                    output.Add(new
                    {
                        Id = item.ToString(),
                        Name = item.Spacify()
                    });
                }
            }
            return output;
        }
    }
}