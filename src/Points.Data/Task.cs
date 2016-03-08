
namespace Points.Data
{
    public class Task : DataBase
    {
        public string CategoryId { get; set; }

        public override void Copy(DataBase obj)
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
