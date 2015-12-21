using Points.Data.Raven;
using Points.Data.View;
using RavenJob = Points.Data.Raven.Job;
using ViewJob = Points.Data.View.Job;

namespace Points.Common.Mappers
{
    public class JobMapper : IObjectMapper<RavenJob, ViewJob>
    {
        private readonly IObjectMapper<RavenObject, ViewObject> _baseMapper;

        public JobMapper(IObjectMapper<RavenObject, ViewObject> baseMapper)
        {
            _baseMapper = baseMapper;
        }

        public ViewJob Map(RavenJob obj)
        {
            var viewJob = new ViewJob();
            viewJob.Copy(_baseMapper.Map(obj));
            viewJob.Processor = obj.Processor;
            viewJob.Trigger = obj.Trigger;
            return viewJob;
        }
    }
}