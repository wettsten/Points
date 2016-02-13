using System;
using System.Collections.Generic;
using System.Web.Http;
using Points.Common.EnumExtensions;

namespace Points.Api.Resources.Controllers
{
    //[Authorize]
    [RoutePrefix("api/enums")]
    public class EnumsController : ApiController
    {
        [Route("")]
        public IHttpActionResult GetEnums()
        {
            var durationTypes = GetEnumsList(typeof(DurationType));
            var durationUnits = GetEnumsList(typeof(DurationUnit));
            var frequencyTypes = GetEnumsList(typeof(FrequencyType));
            var frequencyUnits = GetEnumsList(typeof(FrequencyUnit));
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
