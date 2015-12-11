using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;
using Points.Data;
using Points.DataAccess;

namespace Points.Api.Resources.Controllers
{
    //[Authorize]
    [RoutePrefix("api/users")]
    public class UsersController : ResourceController<User>
    {
        public UsersController(IDataReader dataReader, IDataWriter dataWriter) : base(dataReader, dataWriter) { }

        [Route("")]
        public IHttpActionResult GetUser()
        {
            return Get();
        }

        [Route("{id}")]
        public IHttpActionResult GetUser(string id)
        {
            return Get(id);
        }

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
                    WeekStartHour = 20
                };
                Add(usr);
                return Ok(usr);
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
            var tasks = DataReader.GetAll<Task>().Where(i => i.CategoryId.Equals(id));
            if (tasks.Any())
            {
                return StatusCode(HttpStatusCode.Conflict);
            }
            return Delete(id);
        }
    }
}
