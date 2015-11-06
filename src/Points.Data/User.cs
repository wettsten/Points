using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Points.Data
{
    public class User : RavenObject
    {
        public string Username { get; set; }
        public string Email { get; set; }
        //add user options here
    }
}
