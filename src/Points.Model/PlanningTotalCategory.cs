
using System.Collections.Generic;

namespace Points.Model
{
    public class PlanningTotalCategory : PlanningTotalBase
    {
        public IEnumerable<PlanningTotalTask> Tasks { get; set; }
    }
}
