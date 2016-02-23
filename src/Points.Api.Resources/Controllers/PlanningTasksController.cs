using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Points.Api.Resources.Extensions;
using Points.Common.Processors;
using Points.Model;

namespace Points.Api.Resources.Controllers
{
    [Authorize]
    [RoutePrefix("api/planningtasks")]
    public class PlanningTasksController : ResourceController<PlanningTask>
    {
        public PlanningTasksController(IRequestProcessor requestProcessor) : base(requestProcessor)
        { }

        [Route("")]
        public IHttpActionResult GetPlanningTasksForUser()
        {
            return GetForUser();
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult AddPlanningTask(PlanningTask planningTask)
        {
            return Add(planningTask);
        }

        [Route("")]
        [HttpPut]
        //[HttpPatch]
        public IHttpActionResult EditPlanningTask(PlanningTask planningTask)
        {
            return Edit(planningTask);
        }

        [Route("")]
        [HttpDelete]
        public IHttpActionResult DeletePlanningTask(string id)
        {
            return Delete(id);
        }
    }
}
