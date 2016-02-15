using System.Web.Http;
using Points.Common.Processors;

namespace Points.Api.Resources.Controllers
{
    [Authorize]
    [RoutePrefix("api/enums")]
    public class EnumsController : ApiController
    {
        private readonly IRequestProcessor _requestProcessor;

        public EnumsController(IRequestProcessor requestProcessor)
        {
            _requestProcessor = requestProcessor;
        }

        [Route("")]
        public IHttpActionResult GetEnums()
        {
            return Ok(new
            {
                dTypes = _requestProcessor.GetEnums("DurationType"),
                dUnits = _requestProcessor.GetEnums("DurationUnit"),
                fTypes = _requestProcessor.GetEnums("FrequencyType"),
                fUnits = _requestProcessor.GetEnums("FrequencyUnit")
            });
        }
    }
}
