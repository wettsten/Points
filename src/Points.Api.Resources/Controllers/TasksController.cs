using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

using Points.Data;

namespace AngularJSAuthentication.ResourceServer.Controllers
{
    //[Authorize]
    [RoutePrefix("api/tasks")]
    public class TasksController : ApiController
    {
        [Route("")]
        public IHttpActionResult Get()
        {
            var tasks = StubList();
            if (!tasks.Any())
            {
                return NotFound();
            }
            return Ok(tasks);
        }

        [Route("{id}")]
        public IHttpActionResult Get(long id)
        {
            var task = StubList().FirstOrDefault(i => i.Id.Equals(id));
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        [Route("")]
        public IHttpActionResult GetForUser(long userid)
        {
            var tasks = StubList().Where(i => i.User.Id.Equals(userid) || !i.IsPrivate);
            if (!tasks.Any())
            {
                return NotFound();
            }
            return Ok(tasks);
        }

        private List<Task> StubList()
        {
            return new List<Task>
            {
                new Task
                {
                    Id = 1,
                    Name = "Do dishes",
                    User = new User
                    {
                        Id = 1,
                        Name = "wettsten"
                    },
                    IsPrivate = false,
                    Category = new Category
                    {
                        Id = 1,
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
                    Id = 2,
                    Name = "Clean bathroom",
                    User = new User
                    {
                        Id = 1,
                        Name = "wettsten"
                    },
                    IsPrivate = true,
                    Category = new Category
                    {
                        Id = 1,
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
                    Id = 3,
                    Name = "Go to gym",
                    User = new User
                    {
                        Id = 2,
                        Name = "scott"
                    },
                    IsPrivate = false,
                    Category = new Category
                    {
                        Id = 2,
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
