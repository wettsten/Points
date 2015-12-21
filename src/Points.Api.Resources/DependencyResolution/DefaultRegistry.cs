// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultRegistry.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Points.Common.Mappers;
using Points.Common.Validators;
using Points.DataAccess;
using Points.Scheduler;
using Points.Scheduler.Processors;
using StructureMap;
using StructureMap.Graph;

namespace Points.Api.Resources.DependencyResolution
{
	
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            Scan(
                scan => {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                    scan.AssembliesFromApplicationBaseDirectory(assembly => !assembly.FullName.StartsWith("System.Web"));
                    scan.LookForRegistries();
                    scan.AssemblyContainingType<IObjectValidator>();
                    scan.AssemblyContainingType<IDataReader>();
                    scan.AssemblyContainingType<IScheduler>();
                    scan.AddAllTypesOf<IObjectValidator>();
                    scan.AddAllTypesOf<IJob>();
                    scan.ConnectImplementationsToTypesClosing(typeof(IObjectMapper <,>));
                });
            For<IObjectMapper<Data.Raven.RavenObject, Data.View.ViewObject>>().Use<ObjectMapper>();
            For<IObjectMapper<Data.Raven.Category, Data.View.Category>>().Use<CategoryMapper>();
            For<IObjectMapper<Data.Raven.Task, Data.View.Task>>().Use<TaskMapper>();
            For<IObjectMapper<Data.Raven.PlanningTask, Data.View.PlanningTask>>().Use<PlanningTaskMapper>();
            For<IObjectMapper<Data.Raven.ActiveTask, Data.View.ActiveTask>>().Use<ActiveTaskMapper>();
            For<IObjectMapper<Data.Raven.ArchivedTask, Data.View.ArchivedTask>>().Use<ArchivedTaskMapper>();
            For<IObjectMapper<Data.Raven.User, Data.View.User>>().Use<UserMapper>();
            For<IObjectMapper<Data.Raven.Job, Data.View.Job>>().Use<JobMapper>();
            ForSingletonOf<IScheduler>().Use<Scheduler.Processors.Scheduler>();
        }
    }
}