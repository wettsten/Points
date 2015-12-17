using System.Web.Http;
using Points.Common.Processors;
using RavenTask = Points.Data.Raven.Task;
using ViewTask = Points.Data.View.Task;

namespace Points.Api.Resources.Controllers
{
    //[Authorize]
    [RoutePrefix("api/tasks")]
    public class TasksController : ResourceController<RavenTask,ViewTask>
    {
        public TasksController(IRequestProcessor requestProcessor) : base(requestProcessor)
        { }

        [Route("")]
        public IHttpActionResult GetTasksForUser(string userid)
        {
            return GetForUser(userid);
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult AddTask(RavenTask task)
        {
            return Add(task);
        }

        [Route("")]
        [HttpPut]
        //[HttpPatch]
        public IHttpActionResult EditTask(RavenTask task)
        {
            return Edit(task);
        }

        [Route("")]
        [HttpDelete]
        public IHttpActionResult DeleteTask(string id)
        {
            return Delete(id);
        }
    }
}
