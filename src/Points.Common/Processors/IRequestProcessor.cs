﻿using System.Collections.Generic;
using Points.Model;

namespace Points.Common.Processors
{
    public interface IRequestProcessor
    {
        void AddData<TView>(TView data, string userId) where TView : ViewObject;
        void EditData<TView>(TView data, string userId) where TView : ViewObject;
        void DeleteData<TView>(TView data, string userId) where TView : ViewObject;
        IList<TView> GetListForUser<TView>(string userId) where TView : ViewObject;
        IEnumerable<ViewObject> GetEnums(string enumType);
        dynamic GetPlanningTotals(string userId);
        dynamic GetActiveTotals(string userId);
    }
}
