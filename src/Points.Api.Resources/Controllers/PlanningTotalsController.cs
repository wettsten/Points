using System.Linq;
using System.Web.Http;
using Points.Api.Resources.Extensions;
using Points.Common.Processors;
using Points.Model;

namespace Points.Api.Resources.Controllers
{
    [Authorize]
    [RoutePrefix("api/planningtotals")]
    public class PlanningTotalsController : ResourceController<PlanningTask>
    {
        public PlanningTotalsController(IRequestProcessor requestProcessor) : base(requestProcessor)
        { }

        [Route("")]
        public IHttpActionResult GetPlanningTotals()
        {
            var tasks = GetForUser();
            if (tasks.IsOk())
            {
                var content = tasks.GetContent<PlanningTask>();
                var cats = content
                    .GroupBy(i => i.Task.Category.Id, task => task)
                    .Select(i => new
                    {
                        Id = i.Key,
                        Name = i.First().Task.Category.Name,
                        Points = i.Count(),
                        Tasks = i.OrderBy(j => j.Name)
                    })
                    .OrderBy(i => i.Name);
                return Ok(new
                {
                    Points = cats.Sum(i => i.Points),
                    Categories = cats
                });
            }
            return tasks;
        }
    }
}
