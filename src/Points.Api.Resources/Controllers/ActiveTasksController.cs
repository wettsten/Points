using System.Linq;
using System.Web.Http;
using Points.Api.Resources.Extensions;
using Points.Common.Processors;
using RavenTask = Points.Data.Raven.ActiveTask;
using ViewTask = Points.Data.View.ActiveTask;

namespace Points.Api.Resources.Controllers
{
    //[Authorize]
    [RoutePrefix("api/activetasks")]
    public class ActiveTasksController : ResourceController<RavenTask,ViewTask>
    {
        public ActiveTasksController(IRequestProcessor requestProcessor) : base(requestProcessor)
        { }

        [Route("")]
        public IHttpActionResult GetActiveTasksForUser()
        {
            var tasks = GetForUser();
            if (tasks.IsOk())
            {
                var content = tasks.GetContent<ViewTask>();
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
    }
}
