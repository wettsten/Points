using Points.Data.Raven;

namespace Points.DataAccess.Writers
{
    public interface IDataWriter
    {
        void Add<TN>(TN obj) where TN : RavenObject;
        void Edit<TU>(TU obj) where TU : RavenObject;
        void Delete<TD>(string id) where TD : RavenObject;
    }
}
