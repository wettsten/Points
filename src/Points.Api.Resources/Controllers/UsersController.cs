using System;
using System.Linq;
using System.Web.Http;
using Points.Common.Processors;
using Points.DataAccess;
using RavenUser = Points.Data.Raven.User;
using ViewUser = Points.Data.View.User;

namespace Points.Api.Resources.Controllers
{
    //[Authorize]
    [RoutePrefix("api/users")]
    public class UsersController : ResourceController<RavenUser,ViewUser>
    {
        private readonly IDataReader _dataReader;

        public UsersController(IRequestProcessor requestProcessor, IDataReader dataReader) : base(requestProcessor)
        {
            _dataReader = dataReader;
        }

        [Route("")]
        public IHttpActionResult GetUserByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Name is required");
            }
            var obj = _dataReader.GetAll<RavenUser>().FirstOrDefault(i => i.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) && !i.IsDeleted);
            if (obj == null)
            {
                var now = DateTime.UtcNow.AddDays(1).AddHours(1);
                var usr = new RavenUser
                {
                    Name = name,
                    Email = string.Empty,
                    WeekStartDay = now.DayOfWeek,
                    WeekStartHour = now.Hour,
                    NotifyWeekStarting = false,
                    NotifyWeekEnding = false
                };
                Add(usr);
                return Ok(usr);
            }
            return Ok(obj);
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
