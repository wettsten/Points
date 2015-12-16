namespace Points.Data.Raven
{
    public class ActiveTask : PlanningTask
    {
        public int TimesCompleted { get; set; }
        public bool IsCompleted => TimesCompleted >= Frequency.Value;

        public override void Copy(RavenObject obj)
        {
            base.Copy(obj);
            var task = obj as ActiveTask;
            if (task != null)
            {
                TimesCompleted = task.TimesCompleted;
            }
        }
    }
}
