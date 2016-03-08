using Points.Data;
using Raven.Client;

namespace Points.DataAccess.Writers
{
    public class SingleSessionDataWriter : ISingleSessionDataWriter
    {
        private readonly IDocumentStore _store;

        public SingleSessionDataWriter(IDocumentStore store)
        {
            _store = store;
        }

        public void Add<TN>(TN obj) where TN : DataBase
        {
            using (var session = _store.OpenSession())
            {
                session.Store(obj);
                session.SaveChanges();
                //var id = session.Advanced.GetDocumentId(obj);
            }
        }

        public void Edit<TN>(TN obj) where TN : DataBase
        {
            using (var session = _store.OpenSession())
            {
                var existingObj = session.Load<TN>(obj.Id);
                existingObj.Copy(obj);
                session.SaveChanges();
            }
        }

        public void Delete<TD>(string id) where TD : DataBase
        {
            using (var session = _store.OpenSession())
            {
                var existingObj = session.Load<TD>(id);
                if (existingObj != null)
                {
                    // object exists
                    session.Delete(id);
                    session.SaveChanges();
                }
            }
        }
    }
}
