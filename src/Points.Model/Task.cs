using System.ComponentModel.DataAnnotations;

namespace Points.Model
{
    public class Task : ViewObject
    {
        [Required]
        public Category Category { get; set; }
    }
}
