using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

using Points.Data;
using Points.DataAccess;

namespace AngularJSAuthentication.ResourceServer.Controllers
{
    //[Authorize]
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private readonly IDataReader _dataReader;
        private readonly IDataWriter _dataWriter;

        public UsersController(IDataReader dataReader, IDataWriter dataWriter)
        {
            _dataReader = dataReader;
            _dataWriter = dataWriter;
        }

        [Route("")]
        public IHttpActionResult Get()
        {
            var users = _dataReader.GetAll<User>();
            if (!users.Any())
            {
                return NotFound();
            }
            return Ok(users);
        }

        [Route("{id}")]
        public IHttpActionResult Get(string id)
        {
            var user = _dataReader.Get<User>(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [Route("")]
        public IHttpActionResult GetByUsername(string username)
        {
            var user = _dataReader.GetAll<User>().FirstOrDefault(i => i.Name.ToLower().Equals(username.ToLower()));
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult AddUser(User user)
        {
            _dataWriter.Add(user);
            return Ok();
        }

        [Route("")]
        [HttpPut]
        [HttpPatch]
        public IHttpActionResult EditUser(User user)
        {
            _dataWriter.Edit(user);
            return Ok();
        }

        [Route("")]
        [HttpDelete]
        public IHttpActionResult DeleteUser(string id)
        {
            _dataWriter.Delete(id);
            return Ok();
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
