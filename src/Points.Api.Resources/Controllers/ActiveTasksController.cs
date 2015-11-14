using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Points.Data;
using Points.DataAccess;

namespace Points.Api.Resources.Controllers
{
    //[Authorize]
    [RoutePrefix("api/activetasks")]
    public class ActiveTasksController : ResourceController<ActiveTask>
    {
        public ActiveTasksController(IDataReader dataReader, IDataWriter dataWriter) : base(dataReader, dataWriter) { }

        [Route("")]
        public IHttpActionResult GetActiveTask()
        {
            return Get();
        }

        [Route("{id}")]
        public IHttpActionResult GetActiveTask(string id)
        {
            return Get(id);
        }

        [Route("")]
        public IHttpActionResult GetActiveTaskByName(string name)
        {
            return GetByName(name);
        }

        [Route("")]
        public IHttpActionResult GetActiveTasksForUser(string userid)
        {
            if (string.IsNullOrWhiteSpace(userid))
            {
                return BadRequest(ModelState);
            }
            var allTasks = DataReader.GetAll<ActiveTask>();
            var tasks = allTasks.Where(i => i.User.Id.Equals(userid)).ToList();
            if (!tasks.Any())
            {
                return NotFound();
            }
            return Ok(tasks);
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult AddActiveTask(ActiveTask activeTask)
        {
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        [Route("")]
        [HttpPut]
        //[HttpPatch]
        public IHttpActionResult EditActiveTask(ActiveTask activeTask)
        {
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        [Route("")]
        [HttpDelete]
        public IHttpActionResult DeleteActiveTask(string id)
        {
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        private List<ActiveTask> StubList()
        {
            return new List<ActiveTask>
            {
                new ActiveTask
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
                    Frequency = new ActiveFrequency
                    {
                        Type = FrequencyType.AtLeast,
                        Value = 3,
                        Unit = FrequencyUnit.Days,
                        Completed = 1
                    }
                },
                new ActiveTask
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
                    Frequency = new ActiveFrequency
                    {
                        Type = FrequencyType.Once,
                        Completed = 1
                    }
                },
                new ActiveTask
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
                    Frequency = new ActiveFrequency
                    {
                        Type = FrequencyType.AtLeast,
                        Value = 4,
                        Unit = FrequencyUnit.Days,
                        Completed = 2
                    }
                }
            };
        }
    }
}
