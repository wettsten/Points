using System.Net;
using Points.Data;
using Raven.Client;

namespace Points.DataAccess
{
    public interface IDataWriter
    {
        HttpStatusCode Add<TN>(TN obj) where TN : RavenObject;
        HttpStatusCode Update<TU>(TU obj) where TU : RavenObject;
        HttpStatusCode Delete<TD>(string id) where TD : RavenObject;
    }
}
