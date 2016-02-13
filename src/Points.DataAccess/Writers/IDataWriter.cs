using System;
using Points.Data;

namespace Points.DataAccess.Writers
{
    public interface IDataWriter
    {
        void Add<TN>(TN obj) where TN : RavenObject;
        void Edit<TU>(TU obj) where TU : RavenObject;
        void Delete<TD>(string id) where TD : RavenObject;
        void Delete(string id, Type objType);
    }
}
