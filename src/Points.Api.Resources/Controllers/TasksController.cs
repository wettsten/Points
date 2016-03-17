using System;
using System.Web.Http;
using Points.Model;

namespace Points.Api.Resources.Controllers
{
    [Authorize]
    [RoutePrefix("api/tasks")]
    public class TasksController : ResourceController<Task>
    {
        [Route("")]
        public IHttpActionResult GetTasksForUser()
        {
            return GetForUser();
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult AddTask(Task task)
        {
            return Add(task);
        }

        [Route("")]
        [HttpPut]
        //[HttpPatch]
        public IHttpActionResult EditTask(Task task)
        {
            return Edit(task);
        }

        [Route("")]
        [HttpDelete]
        public IHttpActionResult DeleteTask(string id)
        {
            return Delete(id);
        }

        [Route("available")]
        public IHttpActionResult GetAvailableTasksForUser()
        {
            string userid = GetUserIdFromToken();
            try
            {
                Logger.Info("Get AvailableTask for user {0}. ", userid);
                return Ok(ReadProcessor.GetAvailableTasks(userid));
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Get AvailableTask for user {0}. unknown error", userid);
                return InternalServerError(ex);
            }
        }
    }
}
