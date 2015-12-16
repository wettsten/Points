
using Points.Data.Raven;

namespace Points.Data.View
{
    public class ViewObject
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsDeleted { get; set; }
        public string UserId { get; set; }

        public virtual void Copy(ViewObject obj)
        {
            Id = obj.Id;
            Name = obj.Name;
            IsPrivate = obj.IsPrivate;
            IsDeleted = obj.IsDeleted;
            UserId = obj.UserId;
        }
    }
}
