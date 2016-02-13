using System;
using System.IO;
using System.Linq;
using System.Web.Http;
using Points.Common.Processors;
using Points.Model;

namespace Points.Api.Resources.Controllers
{
    public class ResourceController<TView> : ApiController where TView : ViewObject
    {
        protected readonly IRequestProcessor _requestProcessor;

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
                obj.UserId = GetUserIdFromHeaders();
                _requestProcessor.AddData<TView>(obj);
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
                obj.UserId = GetUserIdFromHeaders();
                _requestProcessor.EditData<TView>(obj);
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
                _requestProcessor.DeleteData<TView>(new ViewObject
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

        protected string GetUserIdFromHeaders()
        {
            return Request.Headers.GetValues("UserId").FirstOrDefault();
        }
    }
}
