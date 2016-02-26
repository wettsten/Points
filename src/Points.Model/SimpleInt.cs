namespace Points.Model
{
    public class SimpleInt
    {
        public int Id { get; set; } 
        public string Name { get; set; }

        public static SimpleInt FromId(int id)
        {
            return new SimpleInt {Id = id};
        }
    }
}