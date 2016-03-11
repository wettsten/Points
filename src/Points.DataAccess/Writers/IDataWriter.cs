using System;
using Points.Data;

namespace Points.DataAccess.Writers
{
    public interface IDataWriter
    {
        void Add<TN>(TN obj) where TN : DataBase;
        void Edit<TU>(TU obj) where TU : DataBase;
        void Delete<TD>(TD obj) where TD : DataBase;
    }
}
