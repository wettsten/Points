
using System.ComponentModel.DataAnnotations;

namespace Points.Model
{
    public class ViewObject
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
