using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Points.Data;
using Raven.Abstractions.Exceptions;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Document.Async;

namespace Points.DataAccess
{
    public class DataWriter : IDataWriter
    {
        private readonly IDocumentStore _store;

        public DataWriter(IDocumentStore store)
        {
            _store = store;
        }

        public bool Add<TN>(TN obj) where TN : RavenObject
        {
            using (var session = _store.OpenSession())
            {
                var existingObj = session.Query<TN>().FirstOrDefault(i => i.Name.Equals(obj.Name));
                if (existingObj != null)
                {
                    // object exists
                    return false;
                }
                session.Store(obj);
                session.SaveChanges();
                var id = session.Advanced.GetDocumentId(obj);
            }
            return true;
        }

        public bool Edit<TU>(TU obj) where TU : RavenObject
        {
            bool exists = true;
            using (var session = _store.OpenSession())
            {
                var existingObj = session.Query<TU>().FirstOrDefault(i => i.Name.Equals(obj.Name));
                if (existingObj == null)
                {
                    // object does not exist
                    exists = false;
                }
                session.Store(obj);
                session.SaveChanges();
            }
            return exists;
        }

        public bool Delete<TD>(string id) where TD : RavenObject
        {
            using (var session = _store.OpenSession())
            {
                var existingObj = session.Load<TD>(id);
                if (existingObj == null)
                {
                    // object does not exist
                    return false;
                }
                session.Delete(id);
                session.SaveChanges();
            }
            return true;
        }
    }
}
