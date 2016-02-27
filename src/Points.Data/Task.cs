
namespace Points.Data
{
    public class Task : RavenObject
    {
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
