using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.DynamicData;
using System.Web.Http;
using System.Web.Http.Results;
using Points.Common.Factories;
using Points.Common.Processors;
using Points.Data;
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
        
        protected IHttpActionResult GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Name is required");
            }
            var objs = _requestProcessor.LookupByName<TIn,TOut>(name);
            if (!objs.Any())
            {
                return NotFound();
            }
            return Ok(objs);
        }
        
        protected IHttpActionResult GetForUser(string userid)
        {
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
                _requestProcessor.DeleteData(new TIn { Id = id});
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
    }
}
