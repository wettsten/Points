using System.Collections.Generic;
using Points.Data;

namespace Points.DataAccess.Readers
{
    public interface ISingleSessionDataReader
    {
        TS Get<TS>(string id) where TS : DataBase;
        IList<TA> GetAll<TA>() where TA : DataBase;
    }
}
