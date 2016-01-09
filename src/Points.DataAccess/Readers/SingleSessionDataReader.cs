﻿using System.Collections.Generic;
using System.Linq;
using Points.Data.Raven;
using Raven.Client;

namespace Points.DataAccess.Readers
{
    public class SingleSessionDataReader : ISingleSessionDataReader
    {
        private readonly IDocumentStore _store;

        public SingleSessionDataReader(IDocumentStore store)
        {
            _store = store;
        }

        public TS Get<TS>(string id) where TS : RavenObject
        {
            using (var session = _store.OpenSession())
            {
                return session.Load<TS>(id);
            }
        }

        public IList<TA> GetAll<TA>() where TA : RavenObject
        {
            using (var session = _store.OpenSession())
            {
                return session.Query<TA>().ToList();
            }
        }
    }
}