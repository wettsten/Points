using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using Points.Common.Processors;
using Points.Model;
using Points.Scheduler.Jobs;
using Points.Scheduler.Processors;

namespace Points.Api.Resources.Controllers
{
    //[Authorize]
    [RoutePrefix("api/users")]
    public class UsersController : ResourceController<User>
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
            var usr = _dataReader.GetAll<RavenUser>().FirstOrDefault(i => i.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            if (usr == null)
            {
                var now = DateTime.UtcNow.AddDays(1).AddHours(1);
                var user = new ViewUser
                {
                    Name = name,
                    Email = string.Empty,
                    WeekStartDay = now.DayOfWeek,
                    WeekStartHour = now.Hour,
                    NotifyWeekStarting = 0,
                    NotifyWeekEnding = 0
                };
                Add(user);
                usr = _dataReader.GetAll<RavenUser>().FirstOrDefault(i => i.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
                EnsureStartJobForUser(usr.Id);
                return Ok(usr);
            }
            EnsureStartJobForUser(usr.Id);
            return Ok(usr);
        }

        [Route("")]
        [HttpPut]
        //[HttpPatch]
        public IHttpActionResult EditUser(User user)
        {
            var result = Edit(user);
            if (result is OkResult)
            {
                _jobProcessor.ScheduleStartJob(user.Id);
            }
            return result;
        }

        private void EnsureStartJobForUser(string userId)
        {
            var job = _dataReader
                .GetAll<RavenJob>()
                .Where(i => i.Processor.Equals(typeof (StartWeekJob).Name, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault(i => i.UserId.Equals(userId, StringComparison.InvariantCultureIgnoreCase));
            if (job == null)
            {
                _jobProcessor.ScheduleStartJob(userId);
            }
        }
    }
}
