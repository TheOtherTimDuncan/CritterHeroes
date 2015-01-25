using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.DataMaintenance.CommandHandlers;
using CritterHeroes.Web.Areas.Common;
using CritterHeroes.Web.Common.Dispatchers;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Common.Proxies;
using CritterHeroes.Web.Common.Proxies.Configuration;
using CritterHeroes.Web.Common.Proxies.Email;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.Dashboard;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Contracts.Notifications;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.DataProviders.Azure;
using CritterHeroes.Web.DataProviders.Azure.Identity;
using CritterHeroes.Web.DataProviders.Azure.Storage;
using CritterHeroes.Web.DataProviders.Azure.Storage.Logging;
using CritterHeroes.Web.DataProviders.RescueGroups.Configuration;
using CritterHeroes.Web.Middleware;
using CritterHeroes.Web.Models;
using FluentValidation;
using Microsoft.Owin;
using SimpleInjector;
using SimpleInjector.Advanced;
using SimpleInjector.Extensions;
using SimpleInjector.Integration.Web.Mvc;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web
{
    public class DIConfig
    {
        public static Container ConfigureDependencyContainer(Assembly additionalAssembly = null)
        {
            Container container = new Container();

            List<Assembly> defaultAssemblies = new List<Assembly>();
            defaultAssemblies.Add(Assembly.GetExecutingAssembly());
            if (additionalAssembly != null)
            {
                defaultAssemblies.Add(additionalAssembly);
            }

            container.RegisterManyForOpenGeneric(typeof(IStateManager<>), defaultAssemblies);
            container.RegisterManyForOpenGeneric(typeof(IMasterStorageContext<>), defaultAssemblies);
            container.RegisterManyForOpenGeneric(typeof(ISecondaryStorageContext<>), defaultAssemblies);

            container.RegisterPerWebRequest<IAppConfiguration, AppConfiguration>();
            container.RegisterPerWebRequest<IAzureConfiguration, AzureConfiguration>();
            container.RegisterPerWebRequest<IEmailConfiguration, EmailConfiguration>();
            container.RegisterPerWebRequest<IRescueGroupsConfiguration, RescueGroupsConfiguration>();
            container.RegisterPerWebRequest<IHttpUser, HttpUserProxy>();

            container.Register<IStateSerializer, StateSerializer>();
            container.Register<IEmailClient, EmailClientProxy>();

            container.RegisterPerWebRequest<ICommandDispatcher, CommandDispatcher>();
            container.RegisterPerWebRequest<IQueryDispatcher, QueryDispatcher>();
            container.RegisterPerWebRequest<INotificationPublisher, NotificationPublisher>();

            container.RegisterPerWebRequest<IHttpContext, HttpContextProxy>();
            container.RegisterPerWebRequest<IUrlGenerator, UrlGenerator>();

            container.Register<IUserLogger, AzureUserLogger>();
            container.Register<IEmailLogger, AzureEmailLogger>();
            container.Register<IStorageContext<Organization>, OrganizationAzureStorage>();

            container.RegisterManyForOpenGeneric(typeof(IValidator<>), defaultAssemblies);

            RegisterIdentityInterfaces(container);
            RegisterHandlers(container, defaultAssemblies);
            RegisterContextSensitiveInterfaces(container);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

            return container;
        }

        public static void RegisterIdentityInterfaces(Container container)
        {
            container.RegisterPerWebRequest<IApplicationUserStore, UserStore>();
            container.RegisterPerWebRequest<IApplicationUserManager, ApplicationUserManager>();
            container.RegisterPerWebRequest<IApplicationSignInManager, ApplicationSignInManager>();
        }

        public static void RegisterHandlers(Container container, IEnumerable<Assembly> defaultAssemblies)
        {
            container.RegisterManyForOpenGeneric(typeof(IQueryHandler<,>), defaultAssemblies);
            container.RegisterManyForOpenGeneric(typeof(IAsyncQueryHandler<,>), defaultAssemblies);
            container.RegisterManyForOpenGeneric(typeof(IDashboardStatusQueryHandler<>), defaultAssemblies);

            container.RegisterManyForOpenGeneric(typeof(ICommandHandler<>), defaultAssemblies);
            container.RegisterManyForOpenGeneric(typeof(IAsyncCommandHandler<>), defaultAssemblies);

            IEnumerable<Type> notificationHandlers =
                from a in defaultAssemblies
                from t in a.GetTypes()
                where t.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(INotificationHandler<>))
                select t;
            container.RegisterAll(typeof(INotificationHandler<>), notificationHandlers);

            IEnumerable<Type> asyncNotificationHandlers =
                from a in defaultAssemblies
                from t in a.GetTypes()
                where t.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IAsyncNotificationHandler<>))
                select t;
            container.RegisterAll(typeof(IAsyncNotificationHandler<>), asyncNotificationHandlers);

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
                return container.GetInstance<IOwinContext>().Authentication;
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

            container.RegisterPerWebRequest(() =>
            {
                if (container.IsVerifying())
                {
                    return new OrganizationContext();
                }

                return container.GetInstance<IOwinContext>().GetOrganizationContext();
            });

            container.RegisterPerWebRequest(() =>
            {
                if (container.IsVerifying())
                {
                    return new UserContext();
                }

                return container.GetInstance<IOwinContext>().GetUserContext();
            });
        }
    }
}