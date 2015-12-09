using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.DynamicData;
using System.Web.Http;
using System.Web.Http.Results;
using Points.Data;
using Points.DataAccess;

namespace Points.Api.Resources.Controllers
{
    public class ResourceController<T> : ApiController where T : RavenObject
    {
        protected readonly IDataReader DataReader;
        protected readonly IDataWriter DataWriter;

        protected ResourceController(IDataReader dataReader, IDataWriter dataWriter)
        {
            DataReader = dataReader;
            DataWriter = dataWriter;
        }
        
        protected IHttpActionResult Get(bool getDeleted = false)
        {
            var objs = DataReader.GetAll<T>().Where(i => !i.IsDeleted || getDeleted);
            if (!objs.Any())
            {
                return NotFound();
            }
            return Ok(objs.OrderBy(i => i.Name));
        }
        
        protected IHttpActionResult Get(string id, bool getDeleted = false)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Id is required");
            }
            var obj = DataReader.Get<T>(id);
            if (obj == null || (obj.IsDeleted || !getDeleted))
            {
                return NotFound();
            }
            return Ok(obj);
        }
        
        protected IHttpActionResult GetByName(string name, bool getDeleted = false)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Name is required");
            }
            var objs = DataReader.GetAll<T>();
            var obj = objs.FirstOrDefault(i => i.Name.ToLower().Equals(name.ToLower()) && (!i.IsDeleted || getDeleted));
            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }
        
        protected IHttpActionResult GetForUser(string userid, bool getDeleted = false)
        {
            if (string.IsNullOrWhiteSpace(userid))
            {
                return BadRequest("User id is required");
            }
            var allObjs = DataReader.GetAll<T>();
            var objs = allObjs.Where(i => (i.UserId.Equals(userid) || !i.IsPrivate) && (!i.IsDeleted || getDeleted)).ToList();
            if (!objs.Any())
            {
                return NotFound();
            }
            return Ok(objs.OrderBy(i => i.Name));
        }

        protected IHttpActionResult Add(T obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrors());
            }
            if (!ValidateUserIdExists(obj.UserId))
            {
                return BadRequest("User id does not exist");
            }
            if (!ValidateObjectNameIsUnique(obj))
            {
                return BadRequest("The name already exists");
            }
            try
            {
                obj.Id = string.Empty;
                return StatusCode(DataWriter.Add(obj));
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
        
        protected IHttpActionResult Edit(T obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrors());
            }
            if (!ValidateUserIdExists(obj.UserId))
            {
                return BadRequest("User id does not exist");
            }
            if (!ValidateObjectNameIsUnique(obj))
            {
                return BadRequest("The name already exists");
            }
            try
            {
                return StatusCode(DataWriter.Update(obj));
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
                return StatusCode(DataWriter.Delete<T>(id));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        private bool ValidateUserIdExists(string userId)
        {
            var user = DataReader.Get<User>(userId);
            return user != null;
        }

        private bool ValidateObjectNameIsUnique(T obj)
        {
            var existingObj = DataReader.GetAll<T>();
            return
                !(existingObj.Any(
                    i =>
                        i.Name.Equals(obj.Name) // same name
                        && (!i.IsPrivate || i.UserId.Equals(obj.UserId)) // object is public or was made by same user
                        && !i.IsDeleted // is not deleted
                        && !i.Id.Equals(obj.Id))); // not the input object
        }

        private string GetModelStateErrors()
        {
            var errors = ModelState.Where(i => i.Value.Errors.Count > 0).SelectMany(i => i.Value.Errors).Select(i => i.ErrorMessage);
            return string.Join("\r\n", errors);
        }
    }
}
