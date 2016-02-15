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
            var tasks = GetForUser();
            if (tasks.IsOk())
            {
                var content = tasks.GetContent<ActiveTask>();
                var cats = content
                    .GroupBy(i => i.Task.Category.Id, task => task)
                    .Select(i => new
                    {
                        Id = i.Key,
                        Name = i.First().Task.Category.Name,
                        Tasks = i.OrderBy(j => j.Task.Name)
                    })
                    .OrderBy(i => i.Name);
                return Ok(cats);
            }
            return tasks;
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
