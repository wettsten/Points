using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Points.Data
{
    public class Task : RavenObject
    {
        public User User { get; set; }
        public Category Category { get; set; }
        public bool IsPrivate { get; set; }
        public Duration Duration { get; set; }
        public Frequency Frequency { get; set; }
    }
}
