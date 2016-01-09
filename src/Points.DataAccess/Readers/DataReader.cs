﻿using System.Collections.Generic;
using System.Linq;
using Points.Data.Raven;
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

        public TS Get<TS>(string id) where TS : RavenObject
        {
            return _session.Load<TS>(id);
        }

        public IList<TA> GetAll<TA>() where TA : RavenObject
        {
            return _session.Query<TA>().ToList();
        }
    }
}