using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using Points.Api.Resources.Extensions;
using Points.Common.Processors;
using Points.Model;
using Points.Scheduler.Jobs;

namespace Points.Api.Resources.Controllers
{
    //[Authorize]
    [RoutePrefix("api/jobs")]
    public class JobsController : ResourceController<Job>
    {
        public JobsController(IRequestProcessor requestProcessor) : base(requestProcessor)
        { }

        [Route("start")]
        public IHttpActionResult GetStartJobForUser()
        {
            var jobs = GetForUser();
            if (jobs.IsOk())
            {
                var content = jobs.GetContent<Job>();
                var job = content.FirstOrDefault(i => i.Processor.Equals(typeof (StartWeekJob).Name));
                if (job == null)
                {
                    return NotFound();
                }
                return Ok(job);
            }
            return jobs;
        }

        [Route("end")]
        public IHttpActionResult GetEndJobForUser()
        {
            var jobs = GetForUser();
            if (jobs.IsOk())
            {
                var content = jobs.GetContent<Job>();
                var job = content.FirstOrDefault(i => i.Processor.Equals(typeof(EndWeekJob).Name));
                if (job == null)
                {
                    return NotFound();
                }
                return Ok(job);
            }
            return jobs;
        }
    }
}
