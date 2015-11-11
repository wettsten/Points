using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Points.Data
{
    public class ActiveTask : Task
    {
        public new ActiveFrequency Frequency { get; set; }
        public bool IsCompleted
        {
            get
            {
                return Frequency.Completed >= Frequency.Value;
            }
        }
    }
}
