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
        private readonly IJobManager _jobProcessor;

        public UsersController(IRequestProcessor requestProcessor, IJobManager jobProcessor) : base(requestProcessor)
        {
            _jobProcessor = jobProcessor;
        }

        [Route("")]
        public IHttpActionResult GetUserByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Name is required");
            }
            var usr = _requestProcessor.GetUser(name);
            if (usr == null)
            {
                var now = DateTime.UtcNow.AddDays(1).AddHours(1);
                usr = new User
                {
                    Name = name,
                    Email = string.Empty,
                    WeekStartDay = now.DayOfWeek,
                    WeekStartHour = now.Hour,
                    NotifyWeekStarting = 0,
                    NotifyWeekEnding = 0
                };
                Add(usr);
                usr = _requestProcessor.GetUser(name);
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
            if (!_requestProcessor
                .GetListForUser<Job>(userId)
                .Any(i => i.Processor.Equals(typeof(StartWeekJob).Name, StringComparison.InvariantCultureIgnoreCase)))
            {
                _jobProcessor.ScheduleStartJob(userId);
            }
        }
    }
}
