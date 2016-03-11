using System.ComponentModel.DataAnnotations;

namespace Points.Model
{
    public class Task : ModelBase
    {
        [Required]
        public Category Category { get; set; }
    }
}
