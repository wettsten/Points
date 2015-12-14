using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Http;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Points.Common.Processors;
using Points.Data;
using Points.Data.EnumExtensions;

namespace Points.Api.Resources.Controllers
{
    //[Authorize]
    [RoutePrefix("api/tasks")]
    public class TasksController : ResourceController<Task>
    {
        public TasksController(IRequestProcessor requestProcessor) : base(requestProcessor)
        { }

        [Route("")]
        public IHttpActionResult GetTaskByName(string name)
        {
            return GetByName(name);
        }

        [Route("")]
        public IHttpActionResult GetTasksForUser(string userid)
        {
            return GetForUser(userid);
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

        [Route("enums")]
        public IHttpActionResult GetEnums()
        {
            var durationTypes = GetEnumsList(typeof (DurationType));
            var durationUnits = GetEnumsList(typeof (DurationUnit));
            var frequencyTypes = GetEnumsList(typeof (FrequencyType));
            var frequencyUnits = GetEnumsList(typeof (FrequencyUnit));
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
