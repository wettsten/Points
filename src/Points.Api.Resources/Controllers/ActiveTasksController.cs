using System.Linq;
using System.Web.Http;
using Points.Api.Resources.Extensions;
using Points.Common.Processors;
using Points.DataAccess.Readers;
using RavenTask = Points.Data.Raven.ActiveTask;
using ViewTask = Points.Data.View.ActiveTask;

namespace Points.Api.Resources.Controllers
{
    //[Authorize]
    [RoutePrefix("api/activetasks")]
    public class ActiveTasksController : ResourceController<RavenTask,ViewTask>
    {
        private DataReader _dataReader;

        public ActiveTasksController(IRequestProcessor requestProcessor, DataReader dataReader) : base(requestProcessor)
        {
            _dataReader = dataReader;
        }

        [Route("")]
        public IHttpActionResult GetActiveTasksForUser()
        {
            var tasks = GetForUser();
            if (tasks.IsOk())
            {
                var content = tasks.GetContent<ViewTask>();
                var cats = content
                    .GroupBy(i => i.Task.Category.Id, task => task)
                    .Select(i => new
                    {
                        Id = i.Key,
                        Name = i.First().Task.Category.Name,
                        Tasks = i.OrderBy(j => j.Task.Name)
                    })
                    .OrderBy(i => i.Name);
                return Ok(cats);
            }
            return tasks;
        }

        [Route("")]
        [HttpPut]
        public IHttpActionResult UpdateTask(RavenTask task)
        {
            var aTask = _dataReader.Get<RavenTask>(task.Id);
            aTask.TimesCompleted = task.TimesCompleted;
            return Edit(aTask);
        }
    }
}
