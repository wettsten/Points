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
        public IWriteProcessor WriteProcessor { get; set; }
        [SetterProperty]
        public IReadProcessor ReadProcessor { get; set; }
        [SetterProperty]
        public ILog Logger { get; set; }

        protected string GetResource => $"Get {typeof (TView).Name} for user {GetUserIdFromToken()}. ";
        protected string AddResource => $"Add {typeof(TView).Name} for user {GetUserIdFromToken()}. ";
        protected string EditResource => $"Edit {typeof(TView).Name} for user {GetUserIdFromToken()}. ";
        protected string DeleteResource => $"Delete {typeof(TView).Name} for user {GetUserIdFromToken()}. ";

        protected IHttpActionResult GetForUser()
        {
            string userid = GetUserIdFromToken();
            Logger.Info(GetResource);
            try
            {
                var objs = ReadProcessor.GetListForUser<TView>(userid);
                Logger.InfoFormat(GetResource + "count: {0}", objs.Count());
                return Ok(objs.OrderBy(i => i.Name));
            }
            catch (Exception ex)
            {
                Logger.Error(GetResource + "unknown error", ex);
                return InternalServerError(ex);
            }
        }

        protected IHttpActionResult Add(TView obj)
        {
            string userid = GetUserIdFromToken();
            Logger.Info(AddResource);
            if (!ModelState.IsValid)
            {
                string errors = GetModelStateErrors();
                Logger.WarnFormat(AddResource + "model state errors: {0}", errors);
                return BadRequest(errors);
            }
            try
            {
                obj.Id = string.Empty;
                WriteProcessor.AddData(obj, userid);
                return Ok();
            }
            catch(InvalidDataException ide)
            {
                Logger.Error(AddResource + "validation error", ide);
                return BadRequest(ide.Message);
            }
            catch (Exception ex)
            {
                Logger.Error(AddResource + "unknown error", ex);
                return InternalServerError(ex);
            }
        }
        
        protected IHttpActionResult Edit(TView obj)
        {
            string userid = GetUserIdFromToken();
            Logger.Info(EditResource);
            if (!ModelState.IsValid)
            {
                string errors = GetModelStateErrors();
                Logger.WarnFormat(EditResource + "model state errors: {0}", errors);
                return BadRequest(errors);
            }
            try
            {
                WriteProcessor.EditData(obj, userid);
                return Ok();
            }
            catch (InvalidDataException ide)
            {
                Logger.Error(EditResource + "validation error", ide);
                return BadRequest(ide.Message);
            }
            catch (Exception ex)
            {
                Logger.Error(EditResource + "unknown error", ex);
                return InternalServerError(ex);
            }
        }
        
        protected IHttpActionResult Delete(string id)
        {
            string userid = GetUserIdFromToken();
            Logger.Info(DeleteResource);
            if (string.IsNullOrWhiteSpace(id))
            {
                Logger.WarnFormat(DeleteResource + "model state errors: Missing object id");
                return BadRequest("Id is required");
            }
            try
            {
                WriteProcessor.DeleteData(new TView { Id = id }, userid);
                return Ok();
            }
            catch (InvalidDataException ide)
            {
                Logger.Error(DeleteResource + "validation error", ide);
                return BadRequest(ide.Message);
            }
            catch (Exception ex)
            {
                Logger.Error(DeleteResource + "unknown error", ex);
                return InternalServerError(ex);
            }
        }

        private string GetModelStateErrors()
        {
            var errors = ModelState.Where(i => i.Value.Errors.Count > 0).SelectMany(i => i.Value.Errors).Select(i => i.ErrorMessage);
            return string.Join("\r\n", errors);
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
