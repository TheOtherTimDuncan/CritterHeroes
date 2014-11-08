using System;
using CH.Azure;
using CH.Domain.Contracts;
using CH.Domain.Handlers;
using CH.Domain.Models;
using CH.Domain.Proxies;
using CH.Domain.StateManagement;
using CH.RescueGroups;
using Ninject;
using Ninject.Modules;

namespace CH.Dependency
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
