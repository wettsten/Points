using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using Autofac;
using Raven.Client;
using Raven.Client.Document;

namespace Points.Api2.Controllers
{
    public abstract class RavenController : ApiController
    {
        private readonly IContainer _container;

        protected RavenController(IContainer container)
        {
            _container = container;
        }

        public IDocumentStore Store => _container.Resolve<IDocumentStore>();

        public IAsyncDocumentSession Session { get; set; }

        public async override Task<HttpResponseMessage> ExecuteAsync(
            HttpControllerContext controllerContext,
            CancellationToken cancellationToken)
        {
            using (Session = Store.OpenAsyncSession())
            {
                var result = await base.ExecuteAsync(controllerContext, cancellationToken);
                await Session.SaveChangesAsync();

                return result;
            }
        }
    }
}