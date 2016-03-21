namespace Points.Scheduler.Processors
{
    public interface IScheduler
    {
        void Start(string keepAliveUrl);
    }
}