using System.Collections.Generic;
using Points.Model;

namespace Points.Common.Processors
{
    public interface IReadProcessor
    {
        IEnumerable<TView> GetListForUser<TView>(string userId) where TView : ModelBase;
        IEnumerable<ModelBase> GetEnums(string enumType);
        PlanningTotal GetPlanningTotals(string userId);
        ActiveTotal GetActiveTotals(string userId);
        IEnumerable<AvailableCategory> GetAvailableTasks(string userId);
    }
}
