using System;
using System.Collections.Generic;
using System.Text;
using Points.Data;
using Points.Data.Raven;

namespace Points.Common.Processors
{
    public interface IRequestProcessor
    {
        void AddData<T>(T data) where T : RavenObject;
        void EditData<T>(T data) where T : RavenObject;
        void DeleteData<T>(T data) where T : RavenObject;
        IList<T> GetListForUser<T>(string userId) where T : RavenObject;
        IList<T> LookupByName<T>(string name) where T : RavenObject;
    }
}
