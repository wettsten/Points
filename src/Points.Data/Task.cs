using System;
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
        public string CategoryId { get; set; }

        public override void Copy(RavenObject obj)
        {
            base.Copy(obj);
            var task = obj as Task;
            if (task != null)
            {
                CategoryId = task.CategoryId;
            }
        }
    }
}
