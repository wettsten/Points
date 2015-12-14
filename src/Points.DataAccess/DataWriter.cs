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

        public void Add<TN>(TN obj) where TN : RavenObject
        {
            _session.Store(obj);
            _session.SaveChanges();
            //var id = session.Advanced.GetDocumentId(obj);
        }

        public void Edit<TN>(TN obj) where TN : RavenObject
        {
            var existingObj = _session.Load<TN>(obj.Id);
            existingObj.Copy(obj);
            _session.SaveChanges();
        }

        public void Delete<TD>(string id, bool hardDelete = false) where TD : RavenObject
        {
            var existingObj = _session.Load<TD>(id);
            if (existingObj != null)
            {
                // object exists
                if (hardDelete)
                {
                    _session.Delete(id);
                }
                else
                {
                    existingObj.IsDeleted = true;
                }
                _session.SaveChanges();
            }
        }
    }
}
