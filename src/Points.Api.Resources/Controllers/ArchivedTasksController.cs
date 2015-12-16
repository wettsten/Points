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
    [RoutePrefix("api/archivedtasks")]
    public class ArchivedTasksController : ResourceController<ArchivedTask>
    {
        public ArchivedTasksController(IRequestProcessor requestProcessor) : base(requestProcessor)
        { }

        [Route("")]
        public IHttpActionResult GetArchivedTaskByName(string name)
        {
            return GetByName(name);
        }

        [Route("")]
        public IHttpActionResult GetArchivedTasksForUser(string userid)
        {
            return GetForUser(userid);
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult AddArchivedTask(ArchivedTask archivedTask)
        {
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        [Route("")]
        [HttpPut]
        //[HttpPatch]
        public IHttpActionResult EditArchivedTask(ArchivedTask archivedTask)
        {
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        [Route("")]
        [HttpDelete]
        public IHttpActionResult DeleteArchivedTask(string id)
        {
            return StatusCode(HttpStatusCode.NotImplemented);
        }
    }
}
