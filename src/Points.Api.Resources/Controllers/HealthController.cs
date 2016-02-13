using System;
using System.Web.Http;
using Points.DataAccess.Readers;
using Points.Model;
using StructureMap;

namespace Points.Api.Resources.Controllers
{
    [RoutePrefix("api/health")]
    [AllowAnonymous]
    public class HealthController : ApiController
    {
        private readonly IContainer _container;

        public HealthController(IContainer container)
        {
            _container = container;
        }

        [Route("")]
        [HttpGet]
        public IHttpActionResult HealthCheck()
        {
            try
            {
                var dataReader = _container.GetInstance<IDataReader>();
                dataReader.GetAll<User>();
            }
            catch (InvalidOperationException ex)
            {
                return InternalServerError(new Exception("Could not connect to Raven Database", ex));
            }
            return Ok();
        }
    }
}
