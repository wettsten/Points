
namespace Points.Data
{
    public class DataBase
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }

        public virtual void Copy(DataBase obj)
        {
            Id = obj.Id;
            Name = obj.Name;
            UserId = obj.UserId;
        }
    }
}
