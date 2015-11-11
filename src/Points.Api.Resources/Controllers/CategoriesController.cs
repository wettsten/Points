using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

using Points.Data;

namespace AngularJSAuthentication.ResourceServer.Controllers
{
    //[Authorize]
    [RoutePrefix("api/categories")]
    public class CategoriesController : ApiController
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

        private List<Category> StubList()
        {
            return new List<Category>
            {
                new Category
                {
                    Id = 1,
                    Name = "Housekeeping"
                },
                new Category
                {
                    Id = 2,
                    Name = "Fitness"
                },
                new Category
                {
                    Id = 3,
                    Name = "Hygiene"
                }
            };
        }
    }
}
