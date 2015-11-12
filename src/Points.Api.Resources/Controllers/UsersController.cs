using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

using Points.Data;

namespace AngularJSAuthentication.ResourceServer.Controllers
{
    //[Authorize]
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        [Route("")]
        public IHttpActionResult Get()
        {
            var cats = StubList();
            if (!cats.Any())
            {
                return NotFound();
            }
            return Ok(cats);
        }

        [Route("{id}")]
        public IHttpActionResult Get(long id)
        {
            var cat = StubList().FirstOrDefault(i => i.Id.Equals(id));
            if (cat == null)
            {
                return NotFound();
            }
            return Ok(cat);
        }

        [Route("")]
        public IHttpActionResult GetByUsername(string username)
        {
            var cat = StubList().FirstOrDefault(i => i.Name.ToLower().Equals(username.ToLower()));
            if (cat == null)
            {
                return NotFound();
            }
            return Ok(cat);
        }

        private List<User> StubList()
        {
            return new List<User>
            {
                new User
                {
                    Id = 1,
                    Name = "wettsten"
                },
                new User
                {
                    Id = 2,
                    Name = "Scott"
                },
                new User
                {
                    Id = 3,
                    Name = "Traci"
                }
            };
        }
    }
}
