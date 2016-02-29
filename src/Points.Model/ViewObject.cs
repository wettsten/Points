
using System.ComponentModel.DataAnnotations;

namespace Points.Model
{
    public class ViewObject
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
