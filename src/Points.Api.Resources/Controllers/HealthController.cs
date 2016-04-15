using System.Web.Http;
using Points.Common.Processors;

namespace Points.Api.Resources.Controllers
{
    [RoutePrefix("api/health")]
    public class HealthController : ApiController
    {
        private readonly IReadProcessor _readProcessor;

        public HealthController(IReadProcessor readProcessor)
        {
            _readProcessor = readProcessor;
        }

        [Route("")]
        [HttpGet]
        public IHttpActionResult HealthCheck()
        {
            return Ok(_readProcessor.GetDocumentCount());
        }
    }
}
