using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using Points.Common.Processors;
using Points.DataAccess;
using Points.Scheduler.Jobs;
using Points.Scheduler.Processors;
using RavenUser = Points.Data.Raven.User;
using ViewUser = Points.Data.View.User;

namespace Points.Api.Resources.Controllers
{
    //[Authorize]
    [RoutePrefix("api/users")]
    public class UsersController : ResourceController<RavenUser,ViewUser>
    {
        private readonly IDataReader _dataReader;
        private readonly IJobManager _jobProcessor;

        public UsersController(IRequestProcessor requestProcessor, IDataReader dataReader, IJobManager jobProcessor) : base(requestProcessor)
        {
            _dataReader = dataReader;
            _jobProcessor = jobProcessor;
        }

        [Route("")]
        public IHttpActionResult GetUserByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Name is required");
            }
            var usr = _dataReader.GetAll<RavenUser>().FirstOrDefault(i => i.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) && !i.IsDeleted);
            if (usr == null)
            {
                var now = DateTime.UtcNow.AddDays(1).AddHours(1);
                usr = new RavenUser
                {
                    Name = name,
                    Email = string.Empty,
                    WeekStartDay = now.DayOfWeek,
                    WeekStartHour = now.Hour,
                    NotifyWeekStarting = false,
                    NotifyWeekEnding = false
                };
                Add(usr);
                usr = _dataReader.GetAll<RavenUser>().FirstOrDefault(i => i.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) && !i.IsDeleted);
                _jobProcessor.ScheduleStartJob(usr.Id);
                return Ok(usr);
            }
            return Ok(usr);
        }

        [Route("")]
        [HttpPut]
        //[HttpPatch]
        public IHttpActionResult EditUser(RavenUser user)
        {
            var result = Edit(user);
            if (result is OkResult)
            {
                _jobProcessor.ScheduleStartJob(user.Id);
            }
            return result;
        }
    }
}
