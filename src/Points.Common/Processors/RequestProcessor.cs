using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Points.Common.Factories;
using Points.Common.Mappers;
using Points.Data;
using Points.Data.Raven;
using Points.Data.View;
using Points.DataAccess;
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
        private readonly IMapperConfiguration _mapperConfiguration;

        public RequestProcessor(IDataReader dataReader, IDataWriter dataWriter, IObjectValidatorFactory objectValidatorFactory, IContainer container, IMapperConfiguration mapperConfiguration)
        {
            _dataReader = dataReader;
            _dataWriter = dataWriter;
            _objectValidatorFactory = objectValidatorFactory;
            _container = container;
            _mapperConfiguration = mapperConfiguration;
        }

        public void AddData<T>(T data) where T : ViewObject
        {
            var validator = _objectValidatorFactory.Get(typeof (T));
            validator?.ValidateAdd(data);
            // map to RavenObject
            _mapperConfiguration.CreateMapper();
            _dataWriter.Add(data);
        }

        public void EditData<T>(T data) where T : ViewObject
        {
            var validator = _objectValidatorFactory.Get(typeof(T));
            validator?.ValidateEdit(data);
            // map to RavenObject
            _dataWriter.Edit(data);
        }

        public void DeleteData<T>(ViewObject data) where T : ViewObject
        {
            var validator = _objectValidatorFactory.Get(typeof(T));
            validator?.ValidateDelete(data);
            // map to RavenObject
            _dataWriter.Delete<T>(data.Id);
        }

        public IList<T> GetListForUser<T>(string userId) where T : ViewObject
        {
            var mapper = _container.GetInstance<IObjectMapper<TIn, TOut>>();
            var objs = _dataReader
                .GetAll<TIn>()
                .Where(i => i.UserId.Equals(userId, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
            return objs.Select(i => mapper.Map(i)).ToList();
        }
    }
}