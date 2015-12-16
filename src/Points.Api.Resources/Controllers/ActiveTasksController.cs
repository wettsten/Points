using System.Web.Http;
using Points.Common.Processors;
using RavenTask = Points.Data.Raven.ActiveTask;
using ViewTask = Points.Data.View.ActiveTask;

namespace Points.Api.Resources.Controllers
{
    //[Authorize]
    [RoutePrefix("api/activetasks")]
    public class ActiveTasksController : ResourceController<RavenTask,ViewTask>
    {
        public ActiveTasksController(IRequestProcessor requestProcessor) : base(requestProcessor)
        { }

        [Route("")]
        public IHttpActionResult GetActiveTaskByName(string name)
        {
            return GetByName(name);
        }

        [Route("")]
        public IHttpActionResult GetActiveTasksForUser(string userid)
        {
            return GetForUser(userid);
        }
    }
}
