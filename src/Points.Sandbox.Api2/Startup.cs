using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using AspNet.Identity.RavenDB.Stores;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Points.Api2.Models;
using Points.Data;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Extensions;

[assembly: OwinStartup(typeof(Points.Api2.Startup))]
namespace Points.Api2
{
    public class Startup
    {
        private IContainer _container;

        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);

            ConfigureRaven(app);
            config.DependencyResolver = new AutofacWebApiDependencyResolver(_container);

            ConfigureOAuth(app);

            app.UseAutofacMiddleware(_container);
            app.UseAutofacWebApi(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = _container.Resolve<IOAuthAuthorizationServerProvider>()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            //app.CreatePerOwinContext<IAsyncDocumentSession>((i,j) => _container.Resolve<IAsyncDocumentSession>(),
            //    (i, j) => { });
            //app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
        }

        public void ConfigureRaven(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.Register(c =>
            {
                IDocumentStore store = new DocumentStore
                {
                    ConnectionStringName = "PointsDB"
                }.Initialize();

                store.DatabaseCommands.EnsureDatabaseExists("Points");

                return store;

            }).As<IDocumentStore>().SingleInstance();

            builder.Register(c =>
            {
                var session = c.Resolve<IDocumentStore>().OpenAsyncSession();
                session.Advanced.UseOptimisticConcurrency = true;
                return session;
            }).As<IAsyncDocumentSession>().InstancePerLifetimeScope();
            
            builder.Register(c => new RavenUserStore<User>(c.Resolve<IAsyncDocumentSession>(), false)).As<IUserStore<User>>().InstancePerLifetimeScope();
            builder.RegisterType<UserManager<User>>().InstancePerLifetimeScope();
            builder.RegisterType<SimpleAuthorizationServerProvider>().As<IOAuthAuthorizationServerProvider>().PropertiesAutowired().SingleInstance();
            builder.RegisterType<AuthRepository>().As<IAuthRepository>().InstancePerLifetimeScope();

            _container = builder.Build();
        }
    }
}