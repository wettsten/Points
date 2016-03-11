using System.Web.Http;
using Points.Common.Processors;
using Points.Model;

namespace Points.Api.Resources.Controllers
{
    [Authorize]
    [RoutePrefix("api/archivedtasks")]
    public class ArchivedTasksController : ResourceController<ArchivedTask>
    {
        [Route("")]
        public IHttpActionResult GetArchivedTasksForUser()
        {
            return GetForUser();
        }
    }
}
