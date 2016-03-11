using System;
using System.Linq;
using System.Web.Http;
using Points.Api.Resources.Extensions;
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
            var tasks = GetForUser();
            if (tasks.IsOk())
            {
                var aTask = tasks.GetContent<ActiveTask>().FirstOrDefault(t => t.Id.Equals(task.Id, StringComparison.InvariantCultureIgnoreCase));
                if (aTask == null)
                {
                    return BadRequest("Task does not exist");
                }
                aTask.TimesCompleted = task.TimesCompleted;
                return Edit(aTask);
            }
            return tasks;
        }

        [Route("totals")]
        public IHttpActionResult GetActiveTotalsForUser()
        {
            try
            {
                return Ok(_requestProcessor.GetActiveTotals(GetUserIdFromToken()));
            }
            catch (InvalidOperationException ide)
            {
                return BadRequest(ide.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
