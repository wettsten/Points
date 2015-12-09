using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Points.Data
{
    public class RavenObject
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsDeleted { get; set; }
        public string UserId { get; set; }

        public virtual void Copy(RavenObject obj)
        {
            Id = obj.Id;
            Name = obj.Name;
            IsPrivate = obj.IsPrivate;
            IsDeleted = obj.IsDeleted;
            UserId = obj.UserId;
        }
    }
}
