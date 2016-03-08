
using System.ComponentModel.DataAnnotations;

namespace Points.Model
{
    public class ModelBase
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
