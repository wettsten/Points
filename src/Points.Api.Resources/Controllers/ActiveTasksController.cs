using System;
using System.Linq;
using System.Web.Http;
using Points.Common.Processors;
using Points.Model;

namespace Points.Api.Resources.Controllers
{
    [Authorize]
    [RoutePrefix("api/activetasks")]
    public class ActiveTasksController : ResourceController<ActiveTask>
    {
        public ActiveTasksController(IRequestProcessor requestProcessor) : base(requestProcessor) { }

        [Route("")]
        public IHttpActionResult GetActiveTasksForUser()
        {
            return GetForUser();
        }

        [Route("")]
        [HttpPut]
        public IHttpActionResult UpdateTask(ActiveTask task)
        {
            var aTask = _requestProcessor
                .GetListForUser<ActiveTask>(GetUserIdFromToken())
                .FirstOrDefault(t => t.Id.Equals(task.Id, StringComparison.InvariantCultureIgnoreCase));
            if (aTask == null)
            {
                return BadRequest("Task does not exist");
            }
            aTask.TimesCompleted = task.TimesCompleted;
            return Edit(aTask);
        }
    }
}
