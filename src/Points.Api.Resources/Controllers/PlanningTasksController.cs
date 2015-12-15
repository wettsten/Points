using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;
using Points.Common.Processors;
using Points.Data;
using Points.Data.EnumExtensions;

namespace Points.Api.Resources.Controllers
{
    //[Authorize]
    [RoutePrefix("api/planningtasks")]
    public class PlanningTasksController : ResourceController<PlanningTask>
    {
        public PlanningTasksController(IRequestProcessor requestProcessor) : base(requestProcessor)
        { }

        [Route("")]
        public IHttpActionResult GetPlanningTaskByName(string name)
        {
            return GetByName(name);
        }

        [Route("")]
        public IHttpActionResult GetPlanningTasksForUser(string userid)
        {
            var tasks = GetForUser(userid);
            //if (tasks is OkResult)
            //{
            //    var content = (tasks as OkResult).Request.Content;
            //}
            return tasks;
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult AddPlanningTask(PlanningTask planningTask)
        {
            return Add(planningTask);
        }

        [Route("")]
        [HttpPut]
        //[HttpPatch]
        public IHttpActionResult EditPlanningTask(PlanningTask planningTask)
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
            var durationTypes = GetEnumsList(typeof(DurationType));
            var durationUnits = GetEnumsList(typeof(DurationUnit));
            var frequencyTypes = GetEnumsList(typeof(FrequencyType));
            var frequencyUnits = GetEnumsList(typeof(FrequencyUnit));
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
