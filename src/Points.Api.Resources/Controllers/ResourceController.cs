using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using log4net;
using Points.Common.Processors;
using Points.Model;
using StructureMap.Attributes;

namespace Points.Api.Resources.Controllers
{
    public abstract class ResourceController<TView> : ApiController where TView : ModelBase, new()
    {
        [SetterProperty]
        public IRequestProcessor RequestProcessor { get; set; }
        [SetterProperty]
        public ILog Logger { get; set; }

        protected IHttpActionResult GetForUser()
        {
            string userid = GetUserIdFromToken();
            Logger.InfoFormat("Getting resource type {0} for user {1}", typeof(TView).Name, userid);
            if (string.IsNullOrWhiteSpace(userid))
            {
                Logger.Warn("Missing user id");
                return BadRequest("User id is required");
            }
            var objs = RequestProcessor.GetListForUser<TView>(userid);
            return Ok(objs.OrderBy(i => i.Name));
        }

        protected IHttpActionResult Add(TView obj)
        {
            string userid = GetUserIdFromToken();
            Logger.InfoFormat("Add resource type {0} for user {1}", typeof(TView).Name, userid);
            if (!ModelState.IsValid)
            {
                string errors = GetModelStateErrors();
                Logger.WarnFormat("Add model state errors: {0}", errors);
                return BadRequest(errors);
            }
            try
            {
                obj.Id = string.Empty;
                RequestProcessor.AddData(obj, userid);
                return Ok();
            }
            catch(InvalidDataException ide)
            {
                Logger.Error("Add validation error", ide);
                return BadRequest(ide.Message);
            }
            catch (Exception ex)
            {
                Logger.Error("Add unknown error", ex);
                return InternalServerError(ex);
            }
        }
        
        protected IHttpActionResult Edit(TView obj)
        {
            string userid = GetUserIdFromToken();
            Logger.InfoFormat("Edit resource type {0} for user {1}", typeof(TView).Name, userid);
            if (!ModelState.IsValid)
            {
                string errors = GetModelStateErrors();
                Logger.WarnFormat("Edit model state errors: {0}", errors);
                return BadRequest(errors);
            }
            try
            {
                RequestProcessor.EditData(obj, userid);
                return Ok();
            }
            catch (InvalidDataException ide)
            {
                Logger.Error("Edit validation error", ide);
                return BadRequest(ide.Message);
            }
            catch (Exception ex)
            {
                Logger.Error("Edit unknown error", ex);
                return InternalServerError(ex);
            }
        }
        
        protected IHttpActionResult Delete(string id)
        {
            string userid = GetUserIdFromToken();
            Logger.InfoFormat("Delete resource type {0} for user {1}", typeof(TView).Name, userid);
            if (string.IsNullOrWhiteSpace(id))
            {
                Logger.Warn("Missing user id");
                return BadRequest("Id is required");
            }
            try
            {
                RequestProcessor.DeleteData(new TView { Id = id }, userid);
                return Ok();
            }
            catch (InvalidDataException ide)
            {
                Logger.Error("Delete validation error", ide);
                return BadRequest(ide.Message);
            }
            catch (Exception ex)
            {
                Logger.Error("Delete unknown error", ex);
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
