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

        public void AddData<TView>(TView data) where TView : ViewObject
        {
            var validator = _objectValidatorFactory.Get(typeof (TView));
            validator?.ValidateAdd(data);
            // map to RavenObject
            var ravenObj = _mapFactory.MapToRavenObject(data);
            _dataWriter.Add(ravenObj);
        }

        public void EditData<TView>(TView data) where TView : ViewObject
        {
            var validator = _objectValidatorFactory.Get(typeof(TView));
            validator?.ValidateEdit(data);
            // map to RavenObject
            var ravenObj = _mapFactory.MapToRavenObject(data);
            _dataWriter.Edit(ravenObj);
        }

        public void DeleteData<TView>(ViewObject data) where TView : ViewObject
        {
            var validator = _objectValidatorFactory.Get(typeof(TView));
            validator?.ValidateDelete(data);
            // map to RavenObject
            var ravenObj = _mapFactory.MapToRavenObject(data);
            _dataWriter.Delete(data.Id, ravenObj.GetType());
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

        public User GetUser(string name)
        {
            var dataUser = _dataReader
                .GetAll<Data.User>()
                .FirstOrDefault(i => i.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            return (User)_mapFactory.MapToViewObject(dataUser);
        }

        public IList<object> GetEnums(string enumType)
        {
            var output = new List<object>();
            var eType = Type.GetType(enumType);
            if (eType != null)
            {
                foreach (var item in Enum.GetValues(eType))
                {
                    output.Add(new
                    {
                        Id = item,
                        Name = item.Spacify()
                    });
                }
            }
            return output;
        }
    }
}