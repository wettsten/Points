using System.Net;
using Points.Data;
using Points.Data.Raven;
using Raven.Client;

namespace Points.DataAccess
{
    public interface IDataWriter
    {
        void Add<TN>(TN obj) where TN : RavenObject;
        void Edit<TU>(TU obj) where TU : RavenObject;
        void Delete<TD>(string id, bool hardDelete = false) where TD : RavenObject;
    }
}
