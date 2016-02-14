using System;
using System.Collections.Generic;
using Points.Model;

namespace Points.Common.Processors
{
    public interface IRequestProcessor
    {
        void AddData<TView>(TView data, string userId) where TView : ViewObject;
        void EditData<TView>(TView data, string userId) where TView : ViewObject;
        void DeleteData<TView>(TView data, string userId) where TView : ViewObject;
        IList<TView> GetListForUser<TView>(string userId) where TView : ViewObject;
        User GetUser(string name);
        IList<object> GetEnums(string enumType);
    }
}
