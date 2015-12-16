using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Points.Data;
using Points.Data.Raven;

namespace Points.DataAccess
{
    public interface IDataReader
    {
        TS Get<TS>(string id) where TS : RavenObject;
        IList<TA> GetAll<TA>() where TA : RavenObject;
    }
}
