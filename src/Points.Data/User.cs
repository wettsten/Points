using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspNet.Identity.RavenDB.Entities;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Raven.Imports.Newtonsoft.Json;

namespace Points.Data
{
    public class User : RavenUser
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        
        //add user options here
        public bool DefaultTasksToPrivate { get; set; }
        public DayOfWeek WeekStartDay { get; set; }
        public int WeekStartHour { get; set; }
        public bool NotifyWeekStarting { get; set; }
        public bool NotifyWeekEnding { get; set; }
    }
}
