using Raven.Client;
using Raven.Client.Document;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace AngularJSAuthentication.ResourceServer.DependencyManagement
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
        }
    }
}