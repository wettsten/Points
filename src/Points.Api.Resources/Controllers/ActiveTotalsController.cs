using System.Linq;
using System.Web.Http;
using Points.Api.Resources.Extensions;
using Points.Common.Processors;
using Points.Model;

namespace Points.Api.Resources.Controllers
{
    [Authorize]
    [RoutePrefix("api/activetotals")]
    public class ActiveTotalsController : ResourceController<ActiveTask>
    {
        public ActiveTotalsController(IRequestProcessor requestProcessor) : base(requestProcessor) { }

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
                        IsCompleted = i.All(j => j.IsCompleted),
                        TargetPoints = i.Count(),
                        TaskPoints = i.Count(j => j.IsCompleted),
                        BonusPoints = i.Sum(j => j.BonusPoints),
                        Tasks = i.Select(j => new
                        {
                            j.Id,
                            j.Name,
                            j.IsCompleted,
                            TargetPoints = 1,
                            TaskPoints = j.IsCompleted ? 1 : 0,
                            j.BonusPoints
                        })
                    })
                    .OrderBy(i => i.Name);
                return Ok(new
                {
                    IsCompleted = cats.All(i => i.IsCompleted),
                    TargetPoints = cats.Sum(i => i.TargetPoints),
                    TaskPoints = cats.Sum(i => i.TaskPoints),
                    BonusPoints = cats.Sum(i => i.BonusPoints),
                    Categories = cats
                });
            }
            return tasks;
        }
    }
}
