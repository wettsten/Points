namespace Points.Model
{
    public class PlanningTask : ViewObject
    {
        public Task Task { get; set; }
        public Duration Duration { get; set; }
        public Frequency Frequency { get; set; }
        public decimal BonusPointValue { get; set; }

        public override void Copy(ViewObject obj)
        {
            base.Copy(obj);
            var task = obj as PlanningTask;
            if (task != null)
            {
                Task = task.Task;
                Duration = task.Duration;
                Frequency = task.Frequency;
            }
        }
    }
}
