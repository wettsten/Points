using System.Collections.Generic;
using Points.Model;

namespace Points.Common.Processors
{
    public interface IRequestProcessor
    {
        void AddData<TView>(TView data, string userId) where TView : ModelBase;
        void EditData<TView>(TView data, string userId) where TView : ModelBase;
        void DeleteData<TView>(TView data, string userId) where TView : ModelBase;
        IList<TView> GetListForUser<TView>(string userId) where TView : ModelBase;
        IEnumerable<ModelBase> GetEnums(string enumType);
        dynamic GetPlanningTotals(string userId);
        dynamic GetActiveTotals(string userId);
    }
}
