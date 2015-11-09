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
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Extensions;

[assembly: OwinStartup(typeof(Points.Api2.Startup))]
namespace Points.Api2
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureOAuth(app);
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);

            var container = ConfigureRaven(app);
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            app.UseAutofacMiddleware(container);
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
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1)
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        }

        public IContainer ConfigureRaven(IAppBuilder app)
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
            }).As<IAsyncDocumentSession>().InstancePerRequest();

            builder.Register(c => new RavenUserStore<Data.User>(c.Resolve<IAsyncDocumentSession>(), false)).As<IUserStore<Data.User>>().InstancePerRequest();
            builder.RegisterType<UserManager<Data.User>>().InstancePerRequest();

            return builder.Build();
        }
    }
}