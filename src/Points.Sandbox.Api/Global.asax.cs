using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AspNet.Identity.RavenDB.Stores;
using Microsoft.AspNet.Identity;
using Points.Api.DependencyResolution;
using Points.Api.IOC;
using Points.Api.Models;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Extensions;
using StructureMap;

namespace Points.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //StructureMap Container
            IContainer container = IoC.Initialize();

            //Register for Web API
            GlobalConfiguration.Configuration.DependencyResolver = new StructureMapWebApiDependencyResolver(container);

            var store = new DocumentStore { ConnectionStringName = "PointsDB" };
            store.Initialize();

            container.For<IDocumentStore>().Use(store).Singleton();
            container.For<IDocumentSession>()
                .HybridHttpOrThreadLocalScoped()
                .Use(c =>
                {
                    var docStore = c.GetInstance<IDocumentStore>();
                    return docStore.OpenSession();
                });
        }
    }
}
