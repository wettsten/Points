using System.ComponentModel.DataAnnotations;

namespace Points.Data.Raven
{
    public class Task : RavenObject
    {
        [Required]
        public string CategoryId { get; set; }

        public override void Copy(RavenObject obj)
        {
            base.Copy(obj);
            var task = obj as Task;
            if (task != null)
            {
                CategoryId = task.CategoryId;
            }
        }
    }
}
