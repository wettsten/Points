using System;
using System.Collections.Generic;
using System.Linq;
using Points.Data;
using Raven.Client;

namespace Points.DataAccess.Readers
{
    public class DataReader : IDataReader
    {
        private readonly IDocumentSession _session;

        public DataReader(IDocumentSession session)
        {
            _session = session;
        }

        public TS Get<TS>(string id) where TS : DataBase
        {
            return _session.Load<TS>(id);
        }

        public IList<TA> GetAll<TA>() where TA : DataBase
        {
            return _session.Query<TA>().ToList();
        }

        public IList<DataBase> GetAll(Type objType)
        {
            return _session.Query<object>(objType.Name).ToList().ConvertAll(i => i as DataBase);
        }
    }
}
