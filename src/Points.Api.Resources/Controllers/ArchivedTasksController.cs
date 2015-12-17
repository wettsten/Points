using System.Web.Http;
using Points.Common.Processors;
using RavenTask = Points.Data.Raven.ArchivedTask;
using ViewTask = Points.Data.View.ArchivedTask;

namespace Points.Api.Resources.Controllers
{
    //[Authorize]
    [RoutePrefix("api/archivedtasks")]
    public class ArchivedTasksController : ResourceController<RavenTask,ViewTask>
    {
        public ArchivedTasksController(IRequestProcessor requestProcessor) : base(requestProcessor)
        { }

        [Route("")]
        public IHttpActionResult GetArchivedTasksForUser(string userid)
        {
            return GetForUser(userid);
        }
    }
}
