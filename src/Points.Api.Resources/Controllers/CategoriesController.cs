﻿using System;
using System.Collections.Generic;
using Points.DataAccess;
using System.Linq;
using System.Web.Http;

using Points.Data;

namespace AngularJSAuthentication.ResourceServer.Controllers
{
    //[Authorize]
    [RoutePrefix("api/categories")]
    public class CategoriesController : ApiController
    {
        private readonly IDataReader _dataReader;
        private readonly IDataWriter _dataWriter;

        public CategoriesController(IDataReader dataReader, IDataWriter dataWriter)
        {
            _dataReader = dataReader;
            _dataWriter = dataWriter;
        }

        [Route("")]
        public IHttpActionResult Get()
        {
            var cats = _dataReader.GetAll<Category>();
            if (!cats.Any())
            {
                return NotFound();
            }
            return Ok(cats);
        }

        [Route("{id}")]
        public IHttpActionResult Get(string id)
        {
            var cat = _dataReader.Get<Category>(id);
            if (cat == null)
            {
                return NotFound();
            }
            return Ok(cat);
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult AddCategory(Category cat)
        {
            _dataWriter.Add(cat);
            return Ok();
        }

        [Route("")]
        [HttpPut]
        [HttpPatch]
        public IHttpActionResult EditCategory(Category cat)
        {
            _dataWriter.Edit(cat);
            return Ok();
        }

        [Route("")]
        [HttpDelete]
        public IHttpActionResult DeleteCategory(string id)
        {
            _dataWriter.Delete(id);
            return Ok();
        }

        private List<Category> StubList()
        {
            return new List<Category>
            {
                new Category
                {
                    Id = "1",
                    Name = "Housekeeping"
                },
                new Category
                {
                    Id = "2",
                    Name = "Fitness"
                },
                new Category
                {
                    Id = "3",
                    Name = "Hygiene"
                }
            };
        }
    }
}