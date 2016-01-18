using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace Points.Api.Auth.Controllers
{
    [RoutePrefix("api/health")]
    [AllowAnonymous]
    public class HealthController : ApiController
    {
        [Route("")]
        [HttpGet]
        public IHttpActionResult HealthCheck()
        {
            try
            {
                var repo = new AuthRepository();
            }
            catch (ProviderIncompatibleException ex)
            {
                return InternalServerError(new Exception("Could not connect to SQL Server Database", ex));
            }
            return Ok();
        }
    }
}
