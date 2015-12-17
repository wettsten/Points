using System.Web.Http;
using Points.Common.Processors;
using RavenCategory = Points.Data.Raven.Category;
using ViewCategory = Points.Data.View.Category;

namespace Points.Api.Resources.Controllers
{
    //[Authorize]
    [RoutePrefix("api/categories")]
    public class CategoriesController : ResourceController<RavenCategory,ViewCategory>
    {
        public CategoriesController(IRequestProcessor requestProcessor) : base(requestProcessor)
        {}

        [Route("")]
        public IHttpActionResult GetCategoriesForUser()
        {
            return GetForUser();
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult AddCategory(RavenCategory category)
        {
            return Add(category);
        }

        [Route("")]
        [HttpPut]
        //[HttpPatch]
        public IHttpActionResult EditCategory(RavenCategory category)
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
