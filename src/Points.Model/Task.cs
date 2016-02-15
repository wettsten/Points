namespace Points.Model
{
    public class Task : ViewObject
    {
        public Category Category { get; set; }

        public override void Copy(ViewObject obj)
        {
            base.Copy(obj);
            var task = obj as Task;
            if (task != null)
            {
                Category = task.Category;
            }
        }
    }
}
