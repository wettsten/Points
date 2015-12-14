using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;
using Points.Common.Processors;
using Points.Data;

namespace Points.Api.Resources.Controllers
{
    //[Authorize]
    [RoutePrefix("api/users")]
    public class UsersController : ResourceController<User>
    {
        public UsersController(IRequestProcessor requestProcessor) : base(requestProcessor)
        { }

        [Route("")]
        public IHttpActionResult GetUserByName(string name)
        {
            var user = GetByName(name);
            if (user is NotFoundResult)
            {
                var usr = new User
                {
                    Name = name,
                    Email = string.Empty,
                    WeekStartDay = DayOfWeek.Sunday,
                    WeekStartHour = 20,
                    NotifyWeekStarting = false,
                    NotifyWeekEnding = false
                };
                Add(usr);
                user = GetByName(name);
            }
            return user;
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult AddUser(User user)
        {
            return Add(user);
        }

        [Route("")]
        [HttpPut]
        //[HttpPatch]
        public IHttpActionResult EditUser(User user)
        {
            return Edit(user);
        }

        [Route("")]
        [HttpDelete]
        public IHttpActionResult DeleteUser(string id)
        {
            return StatusCode(HttpStatusCode.NotImplemented);
        }
    }
}
