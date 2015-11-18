using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Points.Data;
using Raven.Abstractions.Exceptions;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Document.Async;

namespace Points.DataAccess
{
    public class DataWriter : IDataWriter
    {
        private readonly IDocumentStore _store;

        public DataWriter(IDocumentStore store)
        {
            _store = store;
        }

        public HttpStatusCode Add<TN>(TN obj) where TN : RavenObject
        {
            using (var session = _store.OpenSession())
            {
                var existingObj = session.Query<TN>();
                // check if public object has same name
                if (existingObj.Any(i => i.Name.Equals(obj.Name) && !i.IsPrivate && !i.IsDeleted))
                {
                    // object exists
                    throw new InvalidDataException("This name is already in use");
                }
                // object is a task
                if (obj is Task)
                {
                    var task = obj as Task;
                    // check if user already has a private object with same name
                    if (existingObj.Any(i => (i as Task).UserId.Equals(task.UserId) && i.Name.Equals(obj.Name) && i.IsPrivate && !i.IsDeleted))
                    {
                        throw new InvalidDataException("You already have a private task with this name");
                    }
                }
                session.Store(obj);
                session.SaveChanges();
                //var id = session.Advanced.GetDocumentId(obj);
            }
            return HttpStatusCode.Created;
        }

        public HttpStatusCode Edit<TU>(TU obj) where TU : RavenObject
        {
            using (var session = _store.OpenSession())
            {
                var existingObj = session.Query<TU>();
                if (existingObj.Any(i => i.Name.Equals(obj.Name) && !i.IsPrivate && !i.IsDeleted && !i.Id.Equals(obj.Id)))
                {
                    throw new InvalidDataException("This name is already in use");
                }
                // object is a task
                if (obj is Task)
                {
                    var task = obj as Task;
                    // check if user already has a private object with same name
                    if (existingObj.Any(i => (i as Task).UserId.Equals(task.UserId) && i.Name.Equals(obj.Name) && i.IsPrivate && !i.IsDeleted && !i.Id.Equals(obj.Id)))
                    {
                        throw new InvalidDataException("You already have a private task with this name");
                    }
                }

                session.Store(obj);
                session.SaveChanges();
                return HttpStatusCode.NoContent;
            }
        }

        public HttpStatusCode Delete<TD>(string id) where TD : RavenObject
        {
            using (var session = _store.OpenSession())
            {
                var existingObj = session.Load<TD>(id);
                if (existingObj == null)
                {
                    // object does not exist
                    return HttpStatusCode.NotFound;
                }
                //session.Delete(id);
                existingObj.IsDeleted = true;
                session.SaveChanges();
                return HttpStatusCode.NoContent;
            }
        }
    }
}
