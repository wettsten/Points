using Points.Data;

namespace Points.DataAccess.Writers
{
    public interface ISingleSessionDataWriter
    {
        void Add<TN>(TN obj) where TN : DataBase;
        void Edit<TU>(TU obj) where TU : DataBase;
        void Delete<TD>(string id) where TD : DataBase;
    }
}
