using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Points.Data;
using Points.DataAccess;

namespace Points.Api.Resources.Controllers
{
    //[Authorize]
    [RoutePrefix("api/activetasks")]
    public class ActiveTasksController : ApiController
    {
        private readonly IDataReader _dataReader;
        private readonly IDataWriter _dataWriter;

        public ActiveTasksController(IDataReader dataReader, IDataWriter dataWriter)
        {
            _dataReader = dataReader;
            _dataWriter = dataWriter;
        }

        [Route("")]
        public IHttpActionResult Get()
        {
            var tasks = _dataReader.GetAll<ActiveTask>();
            if (!tasks.Any())
            {
                return NotFound();
            }
            return Ok(tasks);
        }

        [Route("{id}")]
        public IHttpActionResult Get(string id)
        {
            var task = _dataReader.Get<ActiveTask>(id);
            if(task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        [Route("")]
        public IHttpActionResult GetForUser(string userid)
        {
            var allTasks = _dataReader.GetAll<ActiveTask>();
            var tasks = allTasks.Where(i => i.User.Id.Equals(userid)).ToList();
            if (!tasks.Any())
            {
                return NotFound();
            }
            return Ok(tasks);
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
