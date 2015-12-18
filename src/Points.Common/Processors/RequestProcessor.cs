using System;
using System.Collections.Generic;
using System.Linq;
using Points.Common.Factories;
using Points.Common.Mappers;
using Points.Data;
using Points.Data.Raven;
using Points.Data.View;
using Points.DataAccess;
using StructureMap;

namespace Points.Common.Processors
{
    public class RequestProcessor : IRequestProcessor
    {
        private readonly IDataReader _dataReader;
        private readonly IDataWriter _dataWriter;
        private readonly IObjectValidatorFactory _objectValidatorFactory;
        private readonly IContainer _container;

        public RequestProcessor(IDataReader dataReader, IDataWriter dataWriter, IObjectValidatorFactory objectValidatorFactory, IContainer container)
        {
            _dataReader = dataReader;
            _dataWriter = dataWriter;
            _objectValidatorFactory = objectValidatorFactory;
            _container = container;
        }

        public void AddData<T>(T data) where T : RavenObject
        {
            var validator = _objectValidatorFactory.Get(typeof (T));
            validator?.ValidateAdd(data);
            _dataWriter.Add(data);
        }

        public void EditData<T>(T data) where T : RavenObject
        {
            var validator = _objectValidatorFactory.Get(typeof(T));
            validator?.ValidateEdit(data);
            _dataWriter.Edit(data);
        }

        public void DeleteData<T>(T data) where T : RavenObject
        {
            var validator = _objectValidatorFactory.Get(typeof(T));
            validator?.ValidateDelete(data);
            _dataWriter.Delete<T>(data.Id);
        }

        public IList<TOut> GetListForUser<TIn,TOut>(string userId) where TIn : RavenObject where TOut : ViewObject
        {
            var mapper = _container.GetInstance<IObjectMapper<TIn, TOut>>();
            var objs = _dataReader
                .GetAll<TIn>()
                .Where(i => i.UserId.Equals(userId, StringComparison.InvariantCultureIgnoreCase))
                .Where(i => !i.IsDeleted)
                .ToList();
            return objs.Select(i => mapper.Map(i)).ToList();
        }
    }
}