using Points.Data;
using Raven.Client;

namespace Points.DataAccess
{
    public interface IDataWriter
    {
        void Add<TN>(TN obj) where TN : RavenObject;
        void Edit<TU>(TU obj) where TU : RavenObject;
        void Delete(string id);
    }
}
