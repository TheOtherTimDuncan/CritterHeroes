using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using CH.Azure.Identity;
using CH.Azure.Storage;
using CH.Azure.Storage.Logging;
using CH.Domain.Contracts;
using CH.Domain.Contracts.Commands;
using CH.Domain.Contracts.Configuration;
using CH.Domain.Contracts.Dashboard;
using CH.Domain.Contracts.Email;
using CH.Domain.Contracts.Identity;
using CH.Domain.Contracts.Logging;
using CH.Domain.Contracts.Queries;
using CH.Domain.Contracts.Storage;
using CH.Domain.Identity;
using CH.Domain.Models;
using CH.Domain.Proxies;
using CH.Domain.Proxies.Configuration;
using CH.Domain.Proxies.Email;
using CH.Domain.Services.CommandHandlers.Dashboard;
using CH.Domain.StateManagement;
using CH.RescueGroups.Configuration;
using CH.Website.Services.Dispatchers;
using CH.Website.Utility;
using SimpleInjector;
using SimpleInjector.Advanced;
using SimpleInjector.Extensions;
using SimpleInjector.Integration.Web.Mvc;
using TOTD.Utility.ExceptionHelpers;

namespace CH.Website
{
    public class DIConfig
    {
        public static Container ConfigureDependencyContainer()
        {
            Container container = new Container();

            List<Assembly> defaultAssemblies = new List<Assembly>();
            defaultAssemblies.Add(typeof(RescueGroupsConfiguration).Assembly);
            defaultAssemblies.Add(typeof(AzureUserLogger).Assembly);
            defaultAssemblies.Add(typeof(IHttpContext).Assembly);
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