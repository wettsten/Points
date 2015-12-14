using System.Collections.Generic;
using Points.Common.Validators;
using StructureMap;

namespace Points.Common.DependencyManagement
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
                    scan.LookForRegistries();
                    scan.AddAllTypesOf<IObjectValidator>();
                });
        }
    }
}