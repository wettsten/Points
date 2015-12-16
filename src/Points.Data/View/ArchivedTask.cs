
namespace Points.Data.View
{
    public class ArchivedTask : ActiveTask
    {
        public override void Copy(ViewObject obj)
        {
            base.Copy(obj);
            var task = obj as ArchivedTask;
            if (task != null)
            {

            }
        }
    }
}
