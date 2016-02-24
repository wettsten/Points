using System;
using System.Collections.Generic;
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
                        WeekStartHour = SimpleInt.FromId(now.Hour),
                        NotifyWeekStarting = SimpleInt.FromId(0),
                        NotifyWeekEnding = SimpleInt.FromId(0),
                        TargetPoints = 0,
                        EnableAdvancedFeatures = false,
                        CategoryBonus = 0,
                        TaskMultiplier = 1,
                        BonusPointMultiplier = 1,
                        DurationBonusPointsPerHour = 0
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

        [Route("days")]
        [HttpGet]
        public IHttpActionResult GetDays()
        {
            return Ok(Enum.GetNames(typeof (DayOfWeek)));
        }

        [Route("hours")]
        [HttpGet]
        public IHttpActionResult GetHours()
        {
            var dt = new DateTime(2000, 1, 1, 0, 0, 0);
            var hours = new List<object>();
            for (int i = 0; i < 24; i++)
            {
                hours.Add(new
                {
                    Id = dt.Hour,
                    Name = dt.ToString("h tt")
                });
                dt = dt.AddHours(1);
            }
            return Ok(hours);
        }

        [Route("hoursprior")]
        [HttpGet]
        public IHttpActionResult GetHoursPrior()
        {
            var hoursPrior = new List<object>
            {
                new
                {
                    Id = 0,
                    Name = "None"
                }
            };
            for (int i = 1; i < 13; i++)
            {
                hoursPrior.Add(new
                {
                    Id = i,
                    Name = string.Format("{0} hour{1}", i, i == 1 ? string.Empty : "s")
                });
            }
            return Ok(hoursPrior);
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
