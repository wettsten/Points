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
        void AddData<T>(T data) where T : ViewObject;
        void EditData<T>(T data) where T : ViewObject;
        void DeleteData<T>(ViewObject data) where T : ViewObject;
        IList<T> GetListForUser<T>(string userId) where T : ViewObject;
    }
}
