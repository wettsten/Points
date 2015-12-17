using System;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;
using Points.Common.Processors;
using RavenUser = Points.Data.Raven.User;
using ViewUser = Points.Data.View.User;

namespace Points.Api.Resources.Controllers
{
    //[Authorize]
    [RoutePrefix("api/users")]
    public class UsersController : ResourceController<RavenUser,ViewUser>
    {
        public UsersController(IRequestProcessor requestProcessor) : base(requestProcessor)
        { }

        [Route("")]
        public IHttpActionResult GetUserByName(string name)
        {
            var user = GetByName(name);
            if (user is NotFoundResult)
            {
                var usr = new RavenUser
                {
                    Name = name,
                    Email = string.Empty,
                    WeekStartDay = DayOfWeek.Sunday,
                    WeekStartHour = 20,
                    NotifyWeekStarting = false,
                    NotifyWeekEnding = false,
                    AllowAdvancedEdit = false
                };
                Add(usr);
                user = GetByName(name);
            }
            return user;
        }

        [Route("")]
        [HttpPut]
        //[HttpPatch]
        public IHttpActionResult EditUser(RavenUser user)
        {
            return Edit(user);
        }
    }
}
