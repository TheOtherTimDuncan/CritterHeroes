﻿using System;
using CH.Azure;
using CH.Azure.Logging;
using CH.Domain.Contracts;
using CH.Domain.Contracts.Configuration;
using CH.Domain.Contracts.Email;
using CH.Domain.Contracts.Logging;
using CH.Domain.Models;
using CH.Domain.Proxies.Configuration;
using CH.Domain.Proxies.Email;
using CH.Domain.StateManagement;
using CH.RescueGroups.Configuration;
using Ninject.Modules;

namespace CH.Dependency.Modules
{
    public class DependencyModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IAppConfiguration>().To<AppConfiguration>();
            Bind<IAzureConfiguration>().To<AzureConfiguration>();
            Bind<IRescueGroupsConfiguration>().To<RescueGroupsConfiguration>();

            Bind<IStorageContext<Organization>>().To<OrganizationAzureStorage>();
            Bind<IStateManager<OrganizationContext>>().To<OrganizationStateManager>();
            Bind<IStateSerializer>().To<StateSerializer>();

            Bind<IEmailConfiguration>().To<EmailConfiguration>();
            Bind<IEmailClient>().To<EmailClientProxy>();

            Bind<IUserLogger>().To<AzureUserLogger>();

            Bind<IAppDependencyResolver>().To<AppDependencyResolver>();
        }
    }
}
