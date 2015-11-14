using Points.Data;
using Raven.Client;

namespace Points.DataAccess
{
    public interface IDataWriter
    {
        bool Add<TN>(TN obj) where TN : RavenObject;
        bool Edit<TU>(TU obj) where TU : RavenObject;
        bool Delete<TD>(string id) where TD : RavenObject;
    }
}
