using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using CH.Azure;
using CH.Azure.Identity;
using CH.Azure.Logging;
using CH.Domain.Contracts;
using CH.Domain.Contracts.Commands;
using CH.Domain.Contracts.Configuration;
using CH.Domain.Contracts.Email;
using CH.Domain.Contracts.Identity;
using CH.Domain.Contracts.Logging;
using CH.Domain.Contracts.Queries;
using CH.Domain.Identity;
using CH.Domain.Models;
using CH.Domain.Proxies;
using CH.Domain.Proxies.Configuration;
using CH.Domain.Proxies.Email;
using CH.Domain.StateManagement;
using CH.RescueGroups;
using CH.RescueGroups.Configuration;
using CH.Website.Services.Dispatchers;
using CH.Website.Utility;
using Microsoft.Owin;
using Microsoft.Owin.Security;
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

            RegisterAzureInterfaces(container);
            RegisterIdentityInterfaces(container);
            RegisterQueryAndCommandHandlers(container);
            RegisterMiscInterfaces(container);
            RegisterRescueGroupsInterfaces(container);
            RegisterWebInterfaces(container);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

            return container;
        }

        public static void RegisterAzureInterfaces(Container container)
        {
            container.RegisterPerWebRequest<IAzureConfiguration, AzureConfiguration>();
            container.Register<IUserLogger, AzureUserLogger>();
            container.Register<IEmailLogger, AzureEmailLogger>();
            container.Register<IStorageContext<Organization>, OrganizationAzureStorage>();
        }

        public static void RegisterIdentityInterfaces(Container container)
        {
            container.RegisterPerWebRequest<IApplicationUserStore, UserStore>();
            container.Register<IApplicationUserManager, ApplicationUserManager>();
            container.Register<IApplicationSignInManager, ApplicationSignInManager>();
        }

        public static void RegisterQueryAndCommandHandlers(Container container)
        {
            container.RegisterPerWebRequest<IQueryDispatcher, QueryDispatcher>();
            container.RegisterPerWebRequest<ICommandDispatcher, CommandDispatcher>();

            container.RegisterManyForOpenGeneric(typeof(IQueryHandler<,>), typeof(IQueryHandler<,>).Assembly, Assembly.GetExecutingAssembly());
            container.RegisterManyForOpenGeneric(typeof(IAsyncQueryHandler<,>), typeof(IQueryHandler<,>).Assembly, Assembly.GetExecutingAssembly());

            container.RegisterManyForOpenGeneric(typeof(ICommandHandler<>), typeof(ICommandHandler<>).Assembly, Assembly.GetExecutingAssembly());
            container.RegisterManyForOpenGeneric(typeof(IAsyncCommandHandler<>), typeof(ICommandHandler<>).Assembly, Assembly.GetExecutingAssembly());
        }

        public static void RegisterMiscInterfaces(Container container)
        {
            container.RegisterPerWebRequest<IAppConfiguration, AppConfiguration>();

            container.Register<IStateManager<OrganizationContext>, OrganizationStateManager>();
            container.Register<IStateManager<UserContext>, UserStateManager>();
            container.Register<IStateSerializer, StateSerializer>();

            container.RegisterPerWebRequest<IEmailConfiguration, EmailConfiguration>();
            container.Register<IEmailClient, EmailClientProxy>();
        }

        public static void RegisterRescueGroupsInterfaces(Container container)
        {
            container.RegisterPerWebRequest<IRescueGroupsConfiguration, RescueGroupsConfiguration>();
            container.Register<IStorageContext, RescueGroupsStorage>();
        }

        public static void RegisterWebInterfaces(Container container)
        {
            container.RegisterPerWebRequest<IHttpContext, HttpContextProxy>();
            container.RegisterPerWebRequest<IUrlGenerator, UrlGenerator>();

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