using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Points.Model
{
    public class PlanningTotal : PlanningTotalBase
    {
        public IEnumerable<PlanningTotalCategory> Categories { get; set; }
    }
}
