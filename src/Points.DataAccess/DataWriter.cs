using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly IDocumentSession _session;

        public DataWriter(IDocumentSession session)
        {
            _session = session;
        }

        public HttpStatusCode Upsert<TN>(TN obj) where TN : RavenObject
        {
            _session.Store(obj);
            _session.SaveChanges();
            //var id = session.Advanced.GetDocumentId(obj);
            return HttpStatusCode.Created;
        }

        public HttpStatusCode Delete<TD>(string id) where TD : RavenObject
        {
            var existingObj = _session.Load<TD>(id);
            if (existingObj == null)
            {
                // object does not exist
                return HttpStatusCode.NotFound;
            }
            //session.Delete(id);
            existingObj.IsDeleted = true;
            _session.SaveChanges();
            return HttpStatusCode.NoContent;
        }
    }
}
