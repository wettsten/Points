using System.Net;
using Points.Data;
using Raven.Client;

namespace Points.DataAccess
{
    public interface IDataWriter
    {
        HttpStatusCode Upsert<TN>(TN obj) where TN : RavenObject;
        HttpStatusCode Delete<TD>(string id) where TD : RavenObject;
    }
}
