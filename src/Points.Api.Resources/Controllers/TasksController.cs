using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            var tasks = allTasks.Where(i => i.User.Id.Equals(userid)).ToList();
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

        private List<Task> StubList()
        {
            return new List<Task>
            {
                new Task
                {
                    Id = "1",
                    Name = "Do dishes",
                    User = new User
                    {
                        Id = "1",
                        Name = "wettsten"
                    },
                    IsPrivate = false,
                    Category = new Category
                    {
                        Id = "1",
                        Name = "Housekeeping"
                    },
                    Duration = new Duration
                    {
                        Type = DurationType.None
                    },
                    Frequency = new Frequency
                    {
                        Type = FrequencyType.AtLeast,
                        Value = 3,
                        Unit = FrequencyUnit.Days
                    }
                },
                new Task
                {
                    Id = "2",
                    Name = "Clean bathroom",
                    User = new User
                    {
                        Id = "1",
                        Name = "wettsten"
                    },
                    IsPrivate = true,
                    Category = new Category
                    {
                        Id = "1",
                        Name = "Housekeeping"
                    },
                    Duration = new Duration
                    {
                        Type = DurationType.None
                    },
                    Frequency = new Frequency
                    {
                        Type = FrequencyType.Once
                    }
                },
                new Task
                {
                    Id = "3",
                    Name = "Go to gym",
                    User = new User
                    {
                        Id = "2",
                        Name = "scott"
                    },
                    IsPrivate = false,
                    Category = new Category
                    {
                        Id = "2",
                        Name = "Fitness"
                    },
                    Duration = new Duration
                    {
                        Type = DurationType.AtLeast,
                        Value = 30,
                        Unit = DurationUnit.Minutes
                    },
                    Frequency = new Frequency
                    {
                        Type = FrequencyType.AtLeast,
                        Value = 4,
                        Unit = FrequencyUnit.Days
                    }
                }
            };
        }
    }
}
