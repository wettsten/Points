using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Http;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Points.Data;
using Points.Data.EnumExtensions;
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
                return BadRequest("User id is required");
            }
            var allTasks = DataReader.GetAll<Task>();
            var tasks = allTasks.Where(i => i.UserId.Equals(userid) || !i.IsPrivate).ToList();
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
            if(!ValidateUserIdExists(task.UserId))
            {
                return BadRequest("User id does not exist");
            }
            if (!ValidateCategoryIdExists(task.CategoryId))
            {
                return BadRequest("Category id does not exist");
            }
            return Add(task);
        }

        [Route("")]
        [HttpPut]
        //[HttpPatch]
        public IHttpActionResult EditTask(Task task)
        {
            if (!ValidateUserIdExists(task.UserId))
            {
                return BadRequest("User id does not exist");
            }
            if (!ValidateCategoryIdExists(task.CategoryId))
            {
                return BadRequest("Category id does not exist");
            }
            return Edit(task);
        }

        [Route("")]
        [HttpDelete]
        public IHttpActionResult DeleteTask(string id)
        {
            return Delete(id);
        }

        private bool ValidateUserIdExists(string userId)
        {
            var user = DataReader.Get<User>(userId);
            return user != null;
        }

        private bool ValidateCategoryIdExists(string catId)
        {
            var user = DataReader.Get<Category>(catId);
            return user != null;
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
