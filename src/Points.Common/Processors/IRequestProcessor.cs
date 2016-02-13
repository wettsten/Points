using System;
using System.Collections.Generic;
using Points.Model;

namespace Points.Common.Processors
{
    public interface IRequestProcessor
    {
        void AddData<TView>(TView data) where TView : ViewObject;
        void EditData<TView>(TView data) where TView : ViewObject;
        void DeleteData<TView>(ViewObject data) where TView : ViewObject;
        IList<TView> GetListForUser<TView>(string userId) where TView : ViewObject;
        User GetUser(string name);
        IList<object> GetEnums(string enumType);
    }
}
