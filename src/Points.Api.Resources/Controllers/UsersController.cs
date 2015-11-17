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
            var tasks = DataReader.GetAll<Task>().Where(i => i.CategoryId.Equals(id));
            if (tasks.Any())
            {
                return StatusCode(HttpStatusCode.Conflict);
            }
            return Delete(id);
        }
    }
}
