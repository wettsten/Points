using Points.Data;

namespace Points.DataAccess.Writers
{
    public interface ISingleSessionDataWriter
    {
        void Add<TN>(TN obj) where TN : RavenObject;
        void Edit<TU>(TU obj) where TU : RavenObject;
        void Delete<TD>(string id) where TD : RavenObject;
    }
}
