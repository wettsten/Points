using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Points.Api.Resources.Extensions;
using Points.Common.Processors;
using Points.Data.EnumExtensions;
using Points.Data.View;
using RavenTask = Points.Data.Raven.PlanningTask;
using ViewTask = Points.Data.View.PlanningTask;

namespace Points.Api.Resources.Controllers
{
    //[Authorize]
    [RoutePrefix("api/planningtasks")]
    public class PlanningTasksController : ResourceController<RavenTask,ViewTask>
    {
        public PlanningTasksController(IRequestProcessor requestProcessor) : base(requestProcessor)
        { }

        [Route("")]
        public IHttpActionResult GetPlanningTasksForUser()
        {
            var tasks = GetForUser();
            if (tasks.IsOk())
            {
                var content = tasks.GetObjects<ViewTask>();
                var cats = content
                    .GroupBy(i => i.Task.Category, task => task)
                    .Select(i => new
                    {
                        Category = i.Key,
                        Tasks = i.AsEnumerable()
                    })
                    .OrderBy(i => i.Category.Name);
                return Ok(cats);
            }
            return tasks;
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult AddPlanningTask(RavenTask planningTask)
        {
            return Add(planningTask);
        }

        [Route("")]
        [HttpPut]
        //[HttpPatch]
        public IHttpActionResult EditPlanningTask(RavenTask planningTask)
        {
            return Edit(planningTask);
        }

        [Route("")]
        [HttpDelete]
        public IHttpActionResult DeletePlanningTask(string id)
        {
            return Delete(id);
        }

        [Route("enums")]
        public IHttpActionResult GetEnums()
        {
            var durationTypes = GetEnumsList(typeof(Data.Raven.DurationType));
            var durationUnits = GetEnumsList(typeof(Data.Raven.DurationUnit));
            var frequencyTypes = GetEnumsList(typeof(Data.Raven.FrequencyType));
            var frequencyUnits = GetEnumsList(typeof(Data.Raven.FrequencyUnit));
            return Ok(new
            {
                dTypes = durationTypes,
                dUnits = durationUnits,
                fTypes = frequencyTypes,
                fUnits = frequencyUnits
            });
        }

        private List<object> GetEnumsList(Type enumType)
        {
            var output = new List<object>();
            foreach (var item in Enum.GetValues(enumType))
            {
                output.Add(new
                {
                    id = item.ToString(),
                    name = item.Spacify()
                });
            }
            return output;
        }
    }
}
