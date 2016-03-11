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

        public void Add<TN>(TN obj) where TN : DataBase
        {
            _session.Store(obj);
            _session.SaveChanges();
            //var id = session.Advanced.GetDocumentId(obj);
        }

        public void Edit<TN>(TN obj) where TN : DataBase
        {
            var existingObj = _session.Load<TN>(obj.Id);
            existingObj.Copy(obj);
            _session.SaveChanges();
        }

        public void Delete<TD>(TD obj) where TD : DataBase
        {
            var existingObj = _session.Load<object>(obj.Id);
            if (existingObj != null && existingObj.GetType() == obj.GetType())
            {
                // object exists
                _session.Delete(obj.Id);
                _session.SaveChanges();
            }
        }
    }
}
