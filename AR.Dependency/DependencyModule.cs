using System;
using AR.Azure;
using AR.Domain.Contracts;
using AR.Domain.Handlers;
using AR.Domain.Models;
using AR.Domain.Proxies;
using AR.Domain.StateManagement;
using AR.RescueGroups;
using Ninject;
using Ninject.Modules;

namespace AR.Dependency
{
    public class DependencyModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IHttpContext>().To<HttpContextProxy>();
            Bind<IAppConfiguration>().To<AppConfiguration>();

            Bind<IStorageContext<Organization>>().To<OrganizationAzureStorage>();
            Bind<IStateManager<OrganizationContext>>().To<OrganizationStateManager>();
        }
    }
}
