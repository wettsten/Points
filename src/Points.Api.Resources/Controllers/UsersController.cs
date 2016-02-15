using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using Points.Api.Resources.Extensions;
using Points.Common.Processors;
using Points.Model;
using Points.Scheduler.Jobs;
using Points.Scheduler.Processors;

namespace Points.Api.Resources.Controllers
{
    [Authorize]
    [RoutePrefix("api/users")]
    public class UsersController : ResourceController<User>
    {
        private readonly IJobManager _jobProcessor;

        public UsersController(IRequestProcessor requestProcessor, IJobManager jobProcessor) : base(requestProcessor)
        {
            _jobProcessor = jobProcessor;
        }

        [Route("")]
        public IHttpActionResult GetUser()
        {
            var usr = GetForUser();
            if (usr.IsOk())
            {
                var user = usr.GetContent<User>().FirstOrDefault();
                if (user == null)
                {
                    var now = DateTime.UtcNow.AddDays(1).AddHours(1);
                    user = new User
                    {
                        Id = GetUserIdFromToken(),
                        Name = GetUserNameFromToken(),
                        Email = string.Empty,
                        WeekStartDay = now.DayOfWeek,
                        WeekStartHour = now.Hour,
                        NotifyWeekStarting = 0,
                        NotifyWeekEnding = 0
                    };
                    _requestProcessor.AddData(user, user.Id);
                    usr = GetForUser();
                }
                EnsureStartJobForUser(user.Id);
                return usr;
            }
            return usr;
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
