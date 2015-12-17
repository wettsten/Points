using System;
using System.IO;
using System.Linq;
using System.Web.Http;
using Points.Common.Processors;
using Points.Data.Raven;
using Points.Data.View;

namespace Points.Api.Resources.Controllers
{
    public class ResourceController<TIn, TOut> : ApiController where TIn : RavenObject, new() where TOut : ViewObject
    {
        private readonly IRequestProcessor _requestProcessor;

        protected ResourceController(IRequestProcessor requestProcessor)
        {
            _requestProcessor = requestProcessor;
        }
        
        protected IHttpActionResult GetForUser()
        {
            string userid = GetUserIdFromHeaders();
            if (string.IsNullOrWhiteSpace(userid))
            {
                return BadRequest("User id is required");
            }
            var objs = _requestProcessor.GetListForUser<TIn,TOut>(userid);
            if (!objs.Any())
            {
                return NotFound();
            }
            return Ok(objs.OrderBy(i => i.Name));
        }

        protected IHttpActionResult Add(TIn obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrors());
            }
            try
            {
                obj.Id = string.Empty;
                obj.UserId = GetUserIdFromHeaders();
                _requestProcessor.AddData(obj);
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
        
        protected IHttpActionResult Edit(TIn obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrors());
            }
            try
            {
                obj.UserId = GetUserIdFromHeaders();
                _requestProcessor.EditData(obj);
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
                _requestProcessor.DeleteData(new TIn
                {
                    Id = id,
                    UserId = GetUserIdFromHeaders()
                });
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

        private string GetUserIdFromHeaders()
        {
            return Request.Headers.GetValues("UserId").FirstOrDefault();
        }
    }
}
