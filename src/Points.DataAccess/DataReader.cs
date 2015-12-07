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
        private readonly IDocumentSession _session;

        public DataReader(IDocumentSession session)
        {
            _session = session;
        }

        public TS Get<TS>(string id) where TS : RavenObject
        {
            return _session.Query<TS>().FirstOrDefault();
        }

        public IList<TA> GetAll<TA>() where TA : RavenObject
        {
            return _session.Query<TA>().ToList();
        }
    }
}
