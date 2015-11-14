using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace Points.Api.Resources.DependencyManagement
{
    public class DependencyRegistry : Registry
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="DependencyRegistry" /> class from being created.
        /// </summary>
        public DependencyRegistry()
        {
            Scan(
                scan =>
                {
                    scan.WithDefaultConventions();
                    scan.AssembliesFromApplicationBaseDirectory(assembly => !assembly.FullName.StartsWith("System.Web"));
                    
                    scan.LookForRegistries();
                });
        }

        /// <summary>
        /// Initialises the IOC registry.
        /// </summary>
        /// <returns>
        /// The initialised IOC container.
        /// </returns>
        public static IContainer Initialise()
        {
            return new Container(x => x.AddRegistry(new DependencyRegistry()));
        }
    }
}