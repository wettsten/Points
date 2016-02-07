using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;

namespace Points.Api.Resources.Extensions
{
    public static class HttpActionResultExtension
    {
        public static IOrderedEnumerable<T> GetObjects<T>(this IHttpActionResult actionResult)
        {
            var ok = actionResult as OkNegotiatedContentResult<IOrderedEnumerable<T>>;
            return ok?.Content;
        }

        public static bool IsOk(this IHttpActionResult actionResult)
        {
            return actionResult.GetType().GetGenericTypeDefinition() == typeof(OkNegotiatedContentResult<>);
        }
    }
}