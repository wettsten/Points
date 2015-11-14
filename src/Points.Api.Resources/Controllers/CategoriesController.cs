using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Points.Data;
using Points.DataAccess;

namespace Points.Api.Resources.Controllers
{
    //[Authorize]
    [RoutePrefix("api/categories")]
    public class CategoriesController : ResourceController<Category>
    {
        public CategoriesController(IDataReader dataReader, IDataWriter dataWriter) : base(dataReader, dataWriter) { }

        [Route("")]
        public IHttpActionResult GetCategory()
        {
            return Get();
        }

        [Route("{id}")]
        public IHttpActionResult GetCategory(string id)
        {
            return Get(id);
        }

        [Route("")]
        public IHttpActionResult GetCategoryByName(string name)
        {
            return GetByName(name);
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult AddCategory(Category category)
        {
            return Add(category);
        }

        [Route("")]
        [HttpPut]
        //[HttpPatch]
        public IHttpActionResult EditCategory(Category category)
        {
            return Edit(category);
        }

        [Route("")]
        [HttpDelete]
        public IHttpActionResult DeleteCategory(string id)
        {
            return Delete(id);
        }

        private List<Category> StubList()
        {
            return new List<Category>
            {
                new Category
                {
                    Id = "1",
                    Name = "Housekeeping"
                },
                new Category
                {
                    Id = "2",
                    Name = "Fitness"
                },
                new Category
                {
                    Id = "3",
                    Name = "Hygiene"
                }
            };
        }
    }
}
