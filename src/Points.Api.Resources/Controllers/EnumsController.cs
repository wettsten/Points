using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Points.Api.Resources.Extensions;
using Points.Common.Processors;
using Points.Data.EnumExtensions;
using Points.Data.View;
using RavenTask = Points.Data.Raven.PlanningTask;
using ViewTask = Points.Data.View.PlanningTask;

namespace Points.Api.Resources.Controllers
{
    //[Authorize]
    [RoutePrefix("api/enums")]
    public class EnumsController : ApiController
    {
        [Route("")]
        public IHttpActionResult GetEnums()
        {
            var durationTypes = GetEnumsList(typeof(Data.Raven.DurationType));
            var durationUnits = GetEnumsList(typeof(Data.Raven.DurationUnit));
            var frequencyTypes = GetEnumsList(typeof(Data.Raven.FrequencyType));
            var frequencyUnits = GetEnumsList(typeof(Data.Raven.FrequencyUnit));
            return Ok(new
            {
                dTypes = durationTypes,
                dUnits = durationUnits,
                fTypes = frequencyTypes,
                fUnits = frequencyUnits
            });
        }

        private List<object> GetEnumsList(Type enumType)
        {
            var output = new List<object>();
            foreach (var item in Enum.GetValues(enumType))
            {
                output.Add(new
                {
                    id = item.ToString(),
                    name = item.Spacify()
                });
            }
            return output;
        }
    }
}
