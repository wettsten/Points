using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Points.Data;
using Raven.Abstractions.Linq;
using Raven.Client;
using Raven.Client.Linq;

namespace Points.DataAccess.Readers
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
            return _session.Load<TS>(id);
        }

        public IList<TA> GetAll<TA>() where TA : RavenObject
        {
            return _session.Query<TA>().ToList();
        }

        public IList<RavenObject> GetAll(Type objType)
        {
            //var x = _session.Advanced
            //    .DocumentQuery<dynamic>("Raven/DocumentsByEntityName")
            //    .Where("Tag:" + objType);
            var ids = _session.Advanced
                .DocumentQuery<dynamic>()
                .SelectFields<dynamic>("@metadata")
                .Select(
                    x => new {Id = x["__document_id"], Type = x["@metadata"]["Raven-Clr-Type"]})
                .Where(i => i.Type.StartsWith(objType.FullName));
            var res = _session.Query<object>()
                .ToList();
            return new List<RavenObject>();
        }
    }
}
