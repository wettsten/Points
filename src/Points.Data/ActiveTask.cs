using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Points.Data
{
    public class ActiveTask : Task
    {
        [Required]
        public Duration Duration { get; set; }
        [Required]
        public Frequency Frequency { get; set; }
        public bool IsCompleted
        {
            get
            {
                return Frequency.Completed >= Frequency.Value;
            }
        }
    }
}
