using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Points.Data;

namespace Points.Api2.Controllers
{
    [RoutePrefix("api/category")]
    public class CategoryController : ApiController
    {
        [Authorize]
        [Route("")]
        public IHttpActionResult Get()
        {
            var categories = new List<Category>
            {
                new Category
                {
                    Id = 1,
                    Name = "Housekeeping"
                },
                new Category
                {
                    Id = 2,
                    Name = "Learning"
                },
                new Category
                {
                    Id = 3,
                    Name = "Finance"
                },
                new Category
                {
                    Id = 4,
                    Name = "Spiritual"
                }
            };
            return Ok(categories);
        }
    }
}