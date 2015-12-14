using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Points.Common.Processors;
using Points.Data;

namespace Points.Api.Resources.Controllers
{
    //[Authorize]
    [RoutePrefix("api/planningtasks")]
    public class PlanningTasksController : ResourceController<PlanningTask>
    {
        public PlanningTasksController(IRequestProcessor requestProcessor) : base(requestProcessor)
        { }

        [Route("")]
        public IHttpActionResult GetPlanningTaskByName(string name)
        {
            return GetByName(name);
        }

        [Route("")]
        public IHttpActionResult GetPlanningTasksForUser(string userid)
        {
            return GetForUser(userid);
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
