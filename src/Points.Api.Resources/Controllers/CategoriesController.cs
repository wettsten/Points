using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Points.Common.Factories;
using Points.Common.Processors;
using Points.Data;

namespace Points.Api.Resources.Controllers
{
    //[Authorize]
    [RoutePrefix("api/categories")]
    public class CategoriesController : ResourceController<Category>
    {
        public CategoriesController(IRequestProcessor requestProcessor) : base(requestProcessor)
        {}

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
            return Delete(id);
        }
    }
}
