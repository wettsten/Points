using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Points.Data;
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

        public void Add<TN>(TN obj) where TN : RavenObject
        {
            using (var session = _store.OpenSession())
            {
                session.Store(obj);
                session.SaveChanges();
                var id = session.Advanced.GetDocumentId(obj);
            }
        }

        public void Edit<TU>(TU obj) where TU : RavenObject
        {
            using (var session = _store.OpenSession())
            {
                session.Store(obj);
                session.SaveChanges();
            }
        }

        public void Delete(string id)
        {
            using (var session = _store.OpenSession())
            {
                session.Delete(id);
                session.SaveChanges();
            }
        }
    }
}
