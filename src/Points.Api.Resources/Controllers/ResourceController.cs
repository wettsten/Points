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
            var objs = DataReader.GetAll<T>();
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
            if (obj == null)
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
            var obj = objs.FirstOrDefault(i => i.Name.ToLower().Equals(name.ToLower()));
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
            bool ok;
            try
            {
                obj.Id = string.Empty;
                ok = DataWriter.Add(obj);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            return ok ? StatusCode(HttpStatusCode.Created) : StatusCode(HttpStatusCode.Conflict);
        }
        
        protected IHttpActionResult Edit(T obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool ok;
            try
            {
                ok = DataWriter.Edit(obj);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            return StatusCode(ok ? HttpStatusCode.NoContent : HttpStatusCode.Created);
        }
        
        protected IHttpActionResult Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(ModelState);
            }
            bool ok;
            try
            {
                ok = DataWriter.Delete<T>(id);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            return ok ? StatusCode(HttpStatusCode.NoContent) : StatusCode(HttpStatusCode.NotFound);
        }
    }
}
