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
                        TotalPoints = i.Count(j => j.IsCompleted) + i.Sum(j => j.BonusPoints),
                        Tasks = i.Select(j => new
                        {
                            j.Id,
                            j.Name,
                            j.IsCompleted,
                            TargetPoints = 1,
                            TaskPoints = j.IsCompleted ? 1 : 0,
                            j.BonusPoints,
                            TotalPoints = (j.IsCompleted ? 1 : 0) + j.BonusPoints
                        }).OrderBy(j => j.Name)
                    })
                    .OrderBy(i => i.Name);
                return Ok(new
                {
                    IsCompleted = cats.All(i => i.IsCompleted),
                    TargetPoints = cats.Sum(i => i.TargetPoints),
                    TaskPoints = cats.Sum(i => i.TaskPoints),
                    BonusPoints = cats.Sum(i => i.BonusPoints),
                    TotalPoints = cats.Sum(i => i.TotalPoints),
                    Categories = cats
                });
            }
            return tasks;
        }
    }
}
