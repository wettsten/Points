using System.Linq;
using System.Web.Http;
using Points.Api.Resources.Extensions;
using Points.Common.Processors;
using Points.DataAccess.Readers;
using RavenTask = Points.Data.Raven.Task;
using ViewTask = Points.Data.View.Task;
using RavenPTask = Points.Data.Raven.PlanningTask;
using ViewPTask = Points.Data.View.PlanningTask;

namespace Points.Api.Resources.Controllers
{
    //[Authorize]
    [RoutePrefix("api/availabletasks")]
    public class AvailableTasksController : ResourceController<RavenTask,ViewTask>
    {
        public AvailableTasksController(IRequestProcessor requestProcessor) : base(requestProcessor)
        { }

        [Route("")]
        public IHttpActionResult GetTasksForUser()
        {
            var tasks = GetForUser();
            if (tasks.IsOk())
            {
                var inUseTasks = _requestProcessor
                    .GetListForUser<RavenPTask,ViewPTask>(GetUserIdFromHeaders())
                    .Select(i => i.Task.Id);
                var content = tasks.GetContent<ViewTask>();
                var cats = content
                    .Where(i => !inUseTasks.Contains(i.Id))
                    .GroupBy(i => i.Category, task => task)
                    .Select(i => new
                    {
                        Id = i.Key.Id,
                        Name = i.Key.Name,
                        Tasks = i.OrderBy(j => j.Name)
                    })
                    .OrderBy(i => i.Name);
                return Ok(cats);
            }
            return tasks;
        }
    }
}
