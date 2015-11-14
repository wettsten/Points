using System.Collections.Generic;
using System.Web.Http;
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
            return GetByName(name);
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
            return Delete(id);
        }

        private List<User> StubList()
        {
            return new List<User>
            {
                new User
                {
                    Id = "1",
                    Name = "wettsten"
                },
                new User
                {
                    Id = "2",
                    Name = "Scott"
                },
                new User
                {
                    Id = "3",
                    Name = "Traci"
                }
            };
        }
    }
}
