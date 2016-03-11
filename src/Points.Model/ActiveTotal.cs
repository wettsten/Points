using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Points.Model
{
    public class ActiveTotal : ActiveTotalBase
    {
        public IEnumerable<ActiveTotalCategory> Categories { get; set; }
    }
}
