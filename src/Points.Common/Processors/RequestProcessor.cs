using System;
using System.Collections.Generic;
using System.Linq;
using Points.Common.Factories;
using Points.Data;
using Points.Data.Raven;
using Points.DataAccess;

namespace Points.Common.Processors
{
    public class RequestProcessor : IRequestProcessor
    {
        private readonly IDataReader _dataReader;
        private readonly IDataWriter _dataWriter;
        private readonly IObjectValidatorFactory _objectValidatorFactory;

        public RequestProcessor(IDataReader dataReader, IDataWriter dataWriter, IObjectValidatorFactory objectValidatorFactory)
        {
            _dataReader = dataReader;
            _dataWriter = dataWriter;
            _objectValidatorFactory = objectValidatorFactory;
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

        public IList<T> GetListForUser<T>(string userId) where T : RavenObject
        {
            return _dataReader.GetAll<T>().Where(i => (i.UserId.Equals(userId) || !i.IsPrivate) && !i.IsDeleted).ToList();
        }

        public IList<T> LookupByName<T>(string name) where T : RavenObject
        {
            return _dataReader.GetAll<T>().Where(i => i.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) && !i.IsDeleted).ToList();
        }
    }
}