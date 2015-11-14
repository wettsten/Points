using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Points.Data;
using Points.DataAccess;

namespace Points.Api.Resources.Controllers
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
            if (!string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(ModelState);
            }
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
            if (!string.IsNullOrWhiteSpace(username))
            {
                return BadRequest(ModelState);
            }
            var users = _dataReader.GetAll<User>();
            var user = users.FirstOrDefault(i => i.Name.ToLower().Equals(username.ToLower()));
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _dataWriter.Add(user);
            }
            catch
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
            return StatusCode(HttpStatusCode.Created);
        }

        [Route("")]
        [HttpPut]
        [HttpPatch]
        public IHttpActionResult EditUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _dataWriter.Edit(user);
            }
            catch
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("")]
        [HttpDelete]
        public IHttpActionResult DeleteUser(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(ModelState);
            }
            try
            {
                _dataWriter.Delete(id);
            }
            catch
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
            return StatusCode(HttpStatusCode.NoContent);
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
