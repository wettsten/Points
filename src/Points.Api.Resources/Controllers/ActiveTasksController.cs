using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Points.Data;
using Points.DataAccess;

namespace Points.Api.Resources.Controllers
{
    //[Authorize]
    [RoutePrefix("api/activetasks")]
    public class ActiveTasksController : ResourceController<ActiveTask>
    {
        public ActiveTasksController(IDataReader dataReader, IDataWriter dataWriter) : base(dataReader, dataWriter) { }

        [Route("")]
        public IHttpActionResult GetActiveTask()
        {
            return Get();
        }

        [Route("{id}")]
        public IHttpActionResult GetActiveTask(string id)
        {
            return Get(id);
        }

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
