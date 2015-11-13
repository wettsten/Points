using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Points.Data;
using Raven.Client;

namespace Points.DataAccess
{
    public class DataReader : IDataReader
    {
        private readonly IDocumentStore _store;

        public DataReader(IDocumentStore store)
        {
            _store = store;
        }

        public TS Get<TS>(string id) where TS : RavenObject
        {
            TS result;
            using (var session = _store.OpenSession())
            {
                result = session.Query<TS>().FirstOrDefault();
            }
            return result;
        }

        public IList<TA> GetAll<TA>() where TA : RavenObject
        {
            IList<TA> result;
            using (var session = _store.OpenSession())
            {
                result = session.Query<TA>().ToList();
            }
            return result;
        }
    }
}
