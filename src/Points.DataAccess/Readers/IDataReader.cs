using System;
using System.Collections.Generic;
using Points.Data;

namespace Points.DataAccess.Readers
{
    public interface IDataReader
    {
        TS Get<TS>(string id) where TS : RavenObject;
        IList<TA> GetAll<TA>() where TA : RavenObject;
        IList<RavenObject> GetAll(Type objType);
    }
}
