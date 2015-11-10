using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using StructureMap;

namespace Points.Api.IOC
{
    public class ServiceActivator : IHttpControllerActivator
    {
        private readonly IContainer _container;
        public ServiceActivator(HttpConfiguration configuration, IContainer container)
        {
            _container = container;
        }

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            var controller = _container.GetInstance(controllerType) as IHttpController;
            return controller;
        }
    }
}