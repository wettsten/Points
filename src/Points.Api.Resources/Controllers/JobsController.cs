using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using Points.Common.Processors;
using Points.Data.EnumExtensions;
using Points.Data.View;
using Points.Scheduler.Jobs;
using RavenJob = Points.Data.Raven.Job;
using ViewJob = Points.Data.View.Job;

namespace Points.Api.Resources.Controllers
{
    //[Authorize]
    [RoutePrefix("api/jobs")]
    public class JobsController : ResourceController<RavenJob, ViewJob>
    {
        public JobsController(IRequestProcessor requestProcessor) : base(requestProcessor)
        { }

        [Route("start")]
        public IHttpActionResult GetStartJobForUser()
        {
            var jobs = GetForUser();
            if (jobs is OkNegotiatedContentResult<IOrderedEnumerable<Job>>)
            {
                var content = (jobs as OkNegotiatedContentResult<IOrderedEnumerable<Job>>).Content;
                var job = content.FirstOrDefault(i => i.Processor.Equals(typeof (StartWeekJob).Name));
                return Ok(job);
            }
            return jobs;
        }

        [Route("end")]
        public IHttpActionResult GetEndJobForUser()
        {
            var jobs = GetForUser();
            if (jobs is OkNegotiatedContentResult<IOrderedEnumerable<Job>>)
            {
                var content = (jobs as OkNegotiatedContentResult<IOrderedEnumerable<Job>>).Content;
                var job = content.FirstOrDefault(i => i.Processor.Equals(typeof(EndWeekJob).Name));
                return Ok(job);
            }
            return jobs;
        }
    }
}
