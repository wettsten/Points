using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Http;
using Points.Data;
using Points.DataAccess;

namespace Points.Api.Resources.Controllers
{
    //[Authorize]
    [RoutePrefix("api/tasks")]
    public class TasksController : ResourceController<Task>
    {
        public TasksController(IDataReader dataReader, IDataWriter dataWriter) : base(dataReader, dataWriter) { }

        [Route("")]
        public IHttpActionResult GetTask()
        {
            return Get();
        }

        [Route("{id}")]
        public IHttpActionResult GetTask(string id)
        {
            return Get(id);
        }

        [Route("")]
        public IHttpActionResult GetTaskByName(string name)
        {
            return GetByName(name);
        }

        [Route("")]
        public IHttpActionResult GetTasksForUser(string userid)
        {
            if (string.IsNullOrWhiteSpace(userid))
            {
                return BadRequest(ModelState);
            }
            var allTasks = DataReader.GetAll<Task>();
            var tasks = allTasks.Where(i => i.UserId.Equals(userid)).ToList();
            if (!tasks.Any())
            {
                return NotFound();
            }
            return Ok(tasks);
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
                    name = Spacify(item.ToString())
                });
            }
            return output;
        } 

        private string Spacify(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }
            StringBuilder output = new StringBuilder(input.Substring(0,1));
            for (int i = 1; i < input.Length; i++)
            {
                if (char.IsUpper(input[i]))
                {
                    output.Append(' ');
                }
                output.Append(input[i]);
            }
            return output.ToString();
        }
    }
}
