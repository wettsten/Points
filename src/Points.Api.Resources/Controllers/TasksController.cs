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
    public class TasksController : ApiController
    {
        private readonly IDataReader _dataReader;
        private readonly IDataWriter _dataWriter;

        public TasksController(IDataReader dataReader, IDataWriter dataWriter)
        {
            _dataReader = dataReader;
            _dataWriter = dataWriter;
        }

        [Route("")]
        public IHttpActionResult Get()
        {
            var tasks = _dataReader.GetAll<Task>();
            if (!tasks.Any())
            {
                return NotFound();
            }
            return Ok(tasks);
        }

        [Route("{id}")]
        public IHttpActionResult Get(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(ModelState);
            }
            var task = _dataReader.Get<Task>(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        [Route("")]
        public IHttpActionResult GetForUser(string userid)
        {
            if (!string.IsNullOrWhiteSpace(userid))
            {
                return BadRequest(ModelState);
            }
            var allTasks = _dataReader.GetAll<Task>();
            var tasks = allTasks.Where(i => i.User.Id.Equals(userid) || !i.IsPrivate).ToList();
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _dataWriter.Add(task);
            }
            catch
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
            return StatusCode(HttpStatusCode.Created);
        }

        [Route("")]
        [HttpPut]
        [HttpPatch]
        public IHttpActionResult EditTask(Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _dataWriter.Edit(task);
            }
            catch
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("")]
        [HttpDelete]
        public IHttpActionResult DeleteTask(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(ModelState);
            }
            try
            {
                _dataWriter.Delete(id);
            }
            catch
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
            return StatusCode(HttpStatusCode.NoContent);
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
