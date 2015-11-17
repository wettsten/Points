using System;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
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
        
        protected IHttpActionResult Get()
        {
            var objs = DataReader.GetAll<T>().Where(i => !i.IsDeleted);
            if (!objs.Any())
            {
                return NotFound();
            }
            return Ok(objs);
        }
        
        protected IHttpActionResult Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(ModelState);
            }
            var obj = DataReader.Get<T>(id);
            if (obj == null || obj.IsDeleted)
            {
                return NotFound();
            }
            return Ok(obj);
        }
        
        protected IHttpActionResult GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest(ModelState);
            }
            var objs = DataReader.GetAll<T>();
            var obj = objs.FirstOrDefault(i => i.Name.ToLower().Equals(name.ToLower()) && !i.IsDeleted);
            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }
        
        protected IHttpActionResult Add(T obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                obj.Id = string.Empty;
                return StatusCode(DataWriter.Add(obj));
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
                return BadRequest(ModelState);
            }
            try
            {
                return StatusCode(DataWriter.Edit(obj));
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
                return BadRequest(ModelState);
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
    }
}
