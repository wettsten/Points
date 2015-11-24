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
        public IHttpActionResult GetCategoriesForUser(string userid)
        {
            return GetForUser(userid);
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
            var tasks = DataReader.GetAll<Task>();
            if (tasks.Any(i => i.CategoryId.Equals(id)))
            {
                return BadRequest("Category is in use");
            }
            return Delete(id);
        }
    }
}
