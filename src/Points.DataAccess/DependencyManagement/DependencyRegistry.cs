using Raven.Client;
using Raven.Client.Document;
using StructureMap;

namespace Points.DataAccess.DependencyManagement
{
    public class DependencyRegistry : Registry
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="DependencyRegistry" /> class from being created.
        /// </summary>
        public DependencyRegistry()
        {
            var docStore = new DocumentStore
            {
                Url = "http://localhost:8080",
                DefaultDatabase = "Points"
            }.Initialize();

            Scan(
                scan =>
                {
                    scan.WithDefaultConventions();
                });
            For<IDocumentStore>().Use(docStore);
            For<IDocumentSession>().Use(ctx => ctx.GetInstance<IDocumentStore>().OpenSession());
        }
    }
}