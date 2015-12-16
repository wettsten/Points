using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Points.Common.Processors;
using Points.Data;
using Points.Data.Raven;

namespace Points.Api.Resources.Controllers
{
    //[Authorize]
    [RoutePrefix("api/activetasks")]
    public class ActiveTasksController : ResourceController<ActiveTask>
    {
        public ActiveTasksController(IRequestProcessor requestProcessor) : base(requestProcessor)
        { }

        [Route("")]
        public IHttpActionResult GetActiveTaskByName(string name)
        {
            return GetByName(name);
        }

        [Route("")]
        public IHttpActionResult GetActiveTasksForUser(string userid)
        {
            return GetForUser(userid);
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult AddActiveTask(ActiveTask activeTask)
        {
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        [Route("")]
        [HttpPut]
        //[HttpPatch]
        public IHttpActionResult EditActiveTask(ActiveTask activeTask)
        {
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        [Route("")]
        [HttpDelete]
        public IHttpActionResult DeleteActiveTask(string id)
        {
            return StatusCode(HttpStatusCode.NotImplemented);
        }
    }
}
