
using System.Collections.Generic;

namespace Points.Model
{
    public class ActiveTotalBase : ModelBase
    {
        public bool IsCompleted { get; set; }
        public int TargetPoints { get; set; }
        public int TaskPoints { get; set; }
        public decimal BonusPoints { get; set; }
        public decimal TotalPoints { get; set; }
    }
}
