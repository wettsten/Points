using System.Web.Http;
using Points.Model;

namespace Points.Api.Resources.Controllers
{
    [Authorize]
    [RoutePrefix("api/categories")]
    public class CategoriesController : ResourceController<Category>
    {
        [Route("")]
        public IHttpActionResult GetCategoriesForUser()
        {
            return GetForUser();
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
    }
}
