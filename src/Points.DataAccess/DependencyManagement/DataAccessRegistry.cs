using Raven.Client;
using Raven.Client.Document;
using StructureMap;
using StructureMap.Graph;

namespace Points.DataAccess.DependencyManagement
{
    public class DataAccessRegistry : Registry
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="DataAccessRegistry" /> class from being created.
        /// </summary>
        public DataAccessRegistry()
        {
            var docStore = new DocumentStore { ConnectionStringName = "PointsRaven" }.Initialize();

            Scan(
                scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });
            For<IDocumentStore>().Use(docStore);
            For<IDocumentSession>().Use(ctx => ctx.GetInstance<IDocumentStore>().OpenSession());
        }
    }
}