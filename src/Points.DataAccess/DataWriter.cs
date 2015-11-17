﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        public HttpStatusCode Add<TN>(TN obj) where TN : RavenObject
        {
            using (var session = _store.OpenSession())
            {
                var existingObj = session.Query<TN>();
                if (existingObj.Any(i => i.Name.Equals(obj.Name) && !i.IsPrivate))
                {
                    // object exists
                    return HttpStatusCode.Conflict;
                }
                session.Store(obj);
                session.SaveChanges();
                //var id = session.Advanced.GetDocumentId(obj);
            }
            return HttpStatusCode.Created;
        }

        public HttpStatusCode Edit<TU>(TU obj) where TU : RavenObject
        {
            using (var session = _store.OpenSession())
            {
                var sameName = session.Query<TU>();
                if (sameName.Any(i => i.Name.Equals(obj.Name) && !i.IsDeleted))
                {
                    return HttpStatusCode.Conflict;
                }

                session.Store(obj);
                session.SaveChanges();
                var existingObj = session.Load<TU>(obj.Id);
                if (existingObj != null)
                {
                    // object exists
                    return HttpStatusCode.NoContent;
                }
                return HttpStatusCode.Created;
            }
        }

        public HttpStatusCode Delete<TD>(string id) where TD : RavenObject
        {
            using (var session = _store.OpenSession())
            {
                var existingObj = session.Load<TD>(id);
                if (existingObj == null)
                {
                    // object does not exist
                    return HttpStatusCode.NotFound;
                }
                //session.Delete(id);
                existingObj.IsDeleted = true;
                session.SaveChanges();
                return HttpStatusCode.NoContent;
            }
        }
    }
}
