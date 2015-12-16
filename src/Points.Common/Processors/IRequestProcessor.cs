using System;
using System.Collections.Generic;
using System.Text;
using Points.Data;
using Points.Data.Raven;
using Points.Data.View;

namespace Points.Common.Processors
{
    public interface IRequestProcessor
    {
        void AddData<T>(T data) where T : RavenObject;
        void EditData<T>(T data) where T : RavenObject;
        void DeleteData<T>(T data) where T : RavenObject;
        IList<TOut> GetListForUser<TIn, TOut>(string userId) where TIn : RavenObject where TOut : ViewObject;
        IList<TOut> LookupByName<TIn, TOut>(string name) where TIn : RavenObject where TOut : ViewObject;
    }
}
