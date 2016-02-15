using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using Points.Common.Processors;
using Points.Model;

namespace Points.Api.Resources.Controllers
{
    public class ResourceController<TView> : ApiController where TView : ViewObject, new()
    {
        protected readonly IRequestProcessor _requestProcessor;

        protected ResourceController(IRequestProcessor requestProcessor)
        {
            _requestProcessor = requestProcessor;
        }
        
        protected IHttpActionResult GetForUser()
        {
            string userid = GetUserIdFromToken();
            if (string.IsNullOrWhiteSpace(userid))
            {
                return BadRequest("User id is required");
            }
            var objs = _requestProcessor.GetListForUser<TView>(userid);
            return Ok(objs.OrderBy(i => i.Name));
        }

        protected IHttpActionResult Add(TView obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrors());
            }
            try
            {
                obj.Id = string.Empty;
                _requestProcessor.AddData(obj, GetUserIdFromToken());
                return Ok();
            }
            catch(InvalidDataException ide)
            {
                return BadRequest(ide.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        
        protected IHttpActionResult Edit(TView obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrors());
            }
            try
            {
                _requestProcessor.EditData(obj, GetUserIdFromToken());
                return Ok();
            }
            catch (InvalidDataException ide)
            {
                return BadRequest(ide.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        
        protected IHttpActionResult Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Id is required");
            }
            try
            {
                _requestProcessor.DeleteData(new TView { Id = id }, GetUserIdFromToken());
                return Ok();
            }
            catch (InvalidDataException ide)
            {
                return BadRequest(ide.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        private string GetModelStateErrors()
        {
            var errors = ModelState.Where(i => i.Value.Errors.Count > 0).SelectMany(i => i.Value.Errors).Select(i => i.ErrorMessage);
            return string.Join("\r\n", errors);
        }

        protected string GetUserIdFromHeaders()
        {
            if (!Request.Headers.Contains("UserId"))
            {
                return string.Empty;
            }
            return Request.Headers.GetValues("UserId").FirstOrDefault();
        }

        protected string GetUserNameFromToken()
        {
            var identity = User.Identity as ClaimsIdentity;
            return identity?.Claims?.FirstOrDefault(i => i.Type.Equals(ClaimTypes.Name))?.Value;
        }

        protected string GetUserIdFromToken()
        {
            var identity = User.Identity as ClaimsIdentity;
            return identity?.Claims?.FirstOrDefault(i => i.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
        }
    }
}
