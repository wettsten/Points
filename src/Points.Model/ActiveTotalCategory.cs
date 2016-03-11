
using System.Collections.Generic;

namespace Points.Model
{
    public class ActiveTotalCategory : ActiveTotalBase
    {
        public IEnumerable<ActiveTotalTask> Tasks { get; set; }
    }
}
