﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Points.Data
{
    public class Task : RavenObject
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string CategoryId { get; set; }
        [Required]
        public Duration Duration { get; set; }
        [Required]
        public Frequency Frequency { get; set; }
    }
}
