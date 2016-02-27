using System.ComponentModel.DataAnnotations;

namespace Points.Model
{
    public class SimpleInt
    {
        [Required]
        public int Id { get; set; } 
        public string Name { get; set; }

        public static SimpleInt FromId(int id)
        {
            return new SimpleInt {Id = id};
        }
    }
}