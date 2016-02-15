using System.Collections.Generic;
using AutoMapper;
using Points.Common.AutoMapper;
using Points.Common.Validators;
using StructureMap;
using Points.DataAccess.Readers;
using StructureMap.Graph;

namespace Points.Common.DependencyManagement
{
    public class CommonRegistry : Registry
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="CommonRegistry" /> class from being created.
        /// </summary>
        public CommonRegistry()
        {
            Scan(
                scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                    scan.LookForRegistries();
                    scan.AssemblyContainingType<IDataReader>();
                    scan.AddAllTypesOf<IObjectValidator>();
                });
            For<IMapper>().Use(ctx => (new MapperConfiguration(mapper => mapper.AddProfile(new MappingProfile(ctx.GetInstance<IDataReader>())))).CreateMapper());
        }
    }
}