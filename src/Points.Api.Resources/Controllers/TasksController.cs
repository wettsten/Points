using System.Linq;
using System.Web.Http;
using Points.Api.Resources.Extensions;
using Points.Common.Processors;
using Points.Model;

namespace Points.Api.Resources.Controllers
{
    [Authorize]
    [RoutePrefix("api/tasks")]
    public class TasksController : ResourceController<Task>
    {
        public TasksController(IRequestProcessor requestProcessor) : base(requestProcessor)
        { }

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
            var tasks = GetForUser();
            if (tasks.IsOk())
            {
                var inUseTasks = _requestProcessor
                    .GetListForUser<PlanningTask>(GetUserIdFromToken())
                    .Select(i => i.Task.Id);
                var content = tasks.GetContent<Task>();
                var cats = content
                    .Where(i => !inUseTasks.Contains(i.Id))
                    .GroupBy(i => i.Category.Id, task => task)
                    .Select(i => new
                    {
                        Id = i.Key,
                        Name = i.First().Category.Name,
                        Tasks = i.OrderBy(j => j.Name)
                    })
                    .OrderBy(i => i.Name);
                return Ok(cats);
            }
            return tasks;
        }
    }
}
