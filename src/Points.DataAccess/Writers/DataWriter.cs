using System;
using Points.Data;
using Raven.Client;

namespace Points.DataAccess.Writers
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

        public void Delete<TD>(string id) where TD : RavenObject
        {
            var existingObj = _session.Load<TD>(id);
            if (existingObj != null)
            {
                // object exists
                _session.Delete(id);
                _session.SaveChanges();
            }
        }

        public void Delete(string id, Type objType)
        {
            var existingObj = _session.Load<object>(id);
            if (existingObj != null && existingObj.GetType() == objType)
            {
                // object exists
                _session.Delete(id);
                _session.SaveChanges();
            }
        }
    }
}
