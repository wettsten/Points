using System;
using System.Linq;
using System.Web.Http;
using Points.Api.Resources.Extensions;
using Points.Model;

namespace Points.Api.Resources.Controllers
{
    [Authorize]
    [RoutePrefix("api/activetasks")]
    public class ActiveTasksController : ResourceController<ActiveTask>
    {
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
                    Logger.Warn("Active task does not exist, cannot edit");
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
            string userid = GetUserIdFromToken();
            try
            {
                Logger.Info("Get ActiveTotals for user {0}. ", userid);
                return Ok(ReadProcessor.GetActiveTotals(userid));
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Get ActiveTotals for user {0}. unknown error", userid);
                return InternalServerError(ex);
            }
        }
    }
}
