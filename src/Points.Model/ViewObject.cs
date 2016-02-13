
namespace Points.Model
{
    public class ViewObject
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }

        public virtual void Copy(ViewObject obj)
        {
            Id = obj.Id;
            Name = obj.Name;
            UserId = obj.UserId;
        }
    }
}
