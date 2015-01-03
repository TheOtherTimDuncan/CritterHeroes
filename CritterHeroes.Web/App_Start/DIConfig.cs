using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Common;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Common.Proxies;
using CritterHeroes.Web.Common.Proxies.Configuration;
using CritterHeroes.Web.Common.Proxies.Email;
using CritterHeroes.Web.Common.Services.CommandHandlers.Dashboard;
using CritterHeroes.Web.Common.Services.Dispatchers;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.Dashboard;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.DataProviders.Azure;
using CritterHeroes.Web.DataProviders.Azure.Identity;
using CritterHeroes.Web.DataProviders.Azure.Storage;
using CritterHeroes.Web.DataProviders.Azure.Storage.Logging;
using CritterHeroes.Web.DataProviders.RescueGroups.Configuration;
using CritterHeroes.Web.Models;
using SimpleInjector;
using SimpleInjector.Advanced;
using SimpleInjector.Extensions;
using SimpleInjector.Integration.Web.Mvc;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web
{
    public class DIConfig
    {
        public static Container ConfigureDependencyContainer()
        {
            Container container = new Container();

            List<Assembly> defaultAssemblies = new List<Assembly>();
            defaultAssemblies.Add(Assembly.GetExecutingAssembly());

            container.RegisterManyForOpenGeneric(typeof(IStateManager<>), defaultAssemblies);
            container.RegisterManyForOpenGeneric(typeof(IMasterStorageContext<>), defaultAssemblies);
            container.RegisterManyForOpenGeneric(typeof(ISecondaryStorageContext<>), defaultAssemblies);

            container.RegisterPerWebRequest<IAppConfiguration, AppConfiguration>();
            container.RegisterPerWebRequest<IAzureConfiguration, AzureConfiguration>();
            container.RegisterPerWebRequest<IEmailConfiguration, EmailConfiguration>();
            container.RegisterPerWebRequest<IRescueGroupsConfiguration, RescueGroupsConfiguration>();

            container.Register<IStateSerializer, StateSerializer>();
            container.Register<IEmailClient, EmailClientProxy>();

            container.RegisterPerWebRequest<ICommandDispatcher, CommandDispatcher>();
            container.RegisterPerWebRequest<IQueryDispatcher, QueryDispatcher>();

            container.RegisterPerWebRequest<IHttpContext, HttpContextProxy>();
            container.RegisterPerWebRequest<IUrlGenerator, UrlGenerator>();

            container.Register<IUserLogger, AzureUserLogger>();
            container.Register<IEmailLogger, AzureEmailLogger>();
            container.Register<IStorageContext<Organization>, OrganizationAzureStorage>();

            RegisterIdentityInterfaces(container);
            RegisterQueryAndCommandHandlers(container, defaultAssemblies);
            RegisterContextSensitiveInterfaces(container);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

            return container;
        }

        public static void RegisterIdentityInterfaces(Container container)
        {
            container.RegisterPerWebRequest<IApplicationUserStore, UserStore>();
            container.Register<IApplicationUserManager, ApplicationUserManager>();
            container.Register<IApplicationSignInManager, ApplicationSignInManager>();
        }

        public static void RegisterQueryAndCommandHandlers(Container container, IEnumerable<Assembly> defaultAssemblies)
        {
            container.RegisterManyForOpenGeneric(typeof(IQueryHandler<,>), defaultAssemblies);
            container.RegisterManyForOpenGeneric(typeof(IAsyncQueryHandler<,>), defaultAssemblies);
            container.RegisterManyForOpenGeneric(typeof(IDashboardStatusQueryHandler<>), defaultAssemblies);

            container.RegisterManyForOpenGeneric(typeof(ICommandHandler<>), defaultAssemblies);
            container.RegisterManyForOpenGeneric(typeof(IAsyncCommandHandler<>), defaultAssemblies);

            container.RegisterManyForOpenGeneric(typeof(IDashboardStatusCommandHandler<>), defaultAssemblies);
            container.ResolveUnregisteredType += (s, e) =>
            {
                if (e.UnregisteredServiceType.IsGenericType && (e.UnregisteredServiceType.GetGenericTypeDefinition() == typeof(IDashboardStatusCommandHandler<>)))
                {
                    Type genericType = typeof(DashboardStatusCommandHandler<>).MakeGenericType(e.UnregisteredServiceType.GetGenericArguments()[0]);
                    InstanceProducer producer = container.GetRegistration(genericType, true);
                    e.Register(producer.Registration);
                }
            };
        }

        public static void RegisterContextSensitiveInterfaces(Container container)
        {
            container.RegisterPerWebRequest(() =>
            {
                if (container.IsVerifying())
                {
                    return new FakeAuthenticationManager();
                }

                ThrowIf.Argument.IsNull(HttpContext.Current, "HttpContext.Current");
                return HttpContext.Current.GetOwinContext().Authentication;
            });

            container.RegisterPerWebRequest(() =>
            {
                if (container.IsVerifying())
                {
                    return new FakeOwinContext();
                }

                ThrowIf.Argument.IsNull(HttpContext.Current, "HttpContext.Current");
                return HttpContext.Current.GetOwinContext();
            });
        }
    }
}