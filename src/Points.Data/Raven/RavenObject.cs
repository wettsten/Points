using System.ComponentModel.DataAnnotations;

namespace Points.Data.Raven
{
    public class RavenObject
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public string UserId { get; set; }

        public virtual void Copy(RavenObject obj)
        {
            Id = obj.Id;
            Name = obj.Name;
            IsDeleted = obj.IsDeleted;
            UserId = obj.UserId;
        }
    }
}
