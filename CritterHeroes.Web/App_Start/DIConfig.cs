using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.Lists.DataMappers;
using CritterHeroes.Web.Areas.Common;
using CritterHeroes.Web.Common.Dispatchers;
using CritterHeroes.Web.Common.Email;
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
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Contexts;
using CritterHeroes.Web.Data.Models.Identity;
using CritterHeroes.Web.Data.Storage;
using CritterHeroes.Web.DataProviders.Azure;
using CritterHeroes.Web.DataProviders.Azure.Storage;
using CritterHeroes.Web.DataProviders.Azure.Storage.Logging;
using CritterHeroes.Web.DataProviders.RescueGroups.Configuration;
using FluentValidation;
using Microsoft.Owin;
using SimpleInjector;
using SimpleInjector.Advanced;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web
{
    public class DIConfig
    {
        public static Container ConfigureDependencyContainer(Assembly additionalAssembly = null)
        {
            Container container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            List<Assembly> defaultAssemblies = new List<Assembly>();
            defaultAssemblies.Add(typeof(DIConfig).Assembly);
            if (additionalAssembly != null)
            {
                defaultAssemblies.Add(additionalAssembly);
            }

            container.Register(typeof(IStateManager<>), defaultAssemblies, Lifestyle.Scoped);
            container.Register(typeof(IAzureStorageContext<>), defaultAssemblies);
            container.Register(typeof(IRescueGroupsStorageContext<>), defaultAssemblies);

            container.RegisterSingleton<IFileSystem, FileSystemProxy>();

            // Register AppUserStorageContext for the one entity SqlStorageContext<> can't handle
            // then register SqlStorageContext<> as a fallback registration for ISqlStorageContext<>
            container.Register<ISqlStorageContext<AppUser>, AppUserStorageContext>(Lifestyle.Scoped);
            container.RegisterConditional(typeof(ISqlStorageContext<>), typeof(SqlStorageContext<>), Lifestyle.Scoped, (c) => !c.Handled);

            container.Register<ICritterBatchSqlStorageContext, CritterBatchStorageContext>(Lifestyle.Scoped);

            container.Register<IAppConfiguration, AppConfiguration>(Lifestyle.Scoped);
            container.Register<IAzureConfiguration, AzureConfiguration>(Lifestyle.Scoped);
            container.Register<IEmailConfiguration, EmailConfiguration>(Lifestyle.Scoped);
            container.Register<IRescueGroupsConfiguration, RescueGroupsConfiguration>(Lifestyle.Scoped);
            container.Register<IHttpUser, HttpUserProxy>(Lifestyle.Scoped);
            container.Register<IHttpContext, HttpContextProxy>(Lifestyle.Scoped);
            container.Register<IUrlGenerator, UrlGenerator>(Lifestyle.Scoped);
            container.Register<IHttpClient, HttpClientProxy>(Lifestyle.Scoped);
            container.Register<IEmailStorageService, EmailStorageService>(Lifestyle.Scoped);
            container.Register<IAzureService, AzureService>(Lifestyle.Scoped);

            container.Register<IPageContextService, PageContextService>(Lifestyle.Scoped);
            container.Register<IStateSerializer, StateSerializer>(Lifestyle.Scoped);

            container.Register<IEmailClient, EmailClientProxy>();
            container.Register<IOrganizationLogoService, OrganizationLogoService>();
            container.Register<ICritterPictureService, CritterPictureService>();
            container.Register<ICommandDispatcher, CommandDispatcher>(Lifestyle.Scoped);
            container.Register<IQueryDispatcher, QueryDispatcher>(Lifestyle.Scoped);
            container.Register<IEmailService, EmailService>();

            container.Register<IUserLogger, AzureUserLogger>();
            container.Register<IEmailLogger, AzureEmailLogger>();

            container.Register(typeof(IValidator<>), defaultAssemblies);

            container.RegisterSingleton<IDataMapperFactory>(new DataMapperFactory() {
                { DataSources.Breed, () => container.GetInstance<BreedDataMapper>() },
                { DataSources.CritterStatus, () => container.GetInstance<CritterStatusMapper>() },
                { DataSources.Species, () => container.GetInstance<SpeciesMapper>() }
            });

            RegisterIdentityInterfaces(container);
            RegisterHandlers(container, defaultAssemblies);
            RegisterContextSensitiveInterfaces(container);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

            return container;
        }

        public static void RegisterIdentityInterfaces(Container container)
        {
            container.Register<IAppSignInManager, AppSignInManager>(Lifestyle.Scoped);
            container.Register<AppUserStorageContext>(() => new AppUserStorageContext(), Lifestyle.Scoped);
            container.Register<IAppUserStore, AppUserStore>(Lifestyle.Scoped);
            container.Register<IAppUserManager, AppUserManager>(Lifestyle.Scoped);
        }

        public static void RegisterHandlers(Container container, IEnumerable<Assembly> defaultAssemblies)
        {
            container.Register(typeof(IQueryHandler<,>), defaultAssemblies);
            container.Register(typeof(IAsyncQueryHandler<,>), defaultAssemblies);

            container.Register(typeof(ICommandHandler<>), defaultAssemblies);
            container.Register(typeof(IAsyncCommandHandler<>), defaultAssemblies);
        }

        public static void RegisterContextSensitiveInterfaces(Container container)
        {
            container.Register(() =>
            {
                if (container.IsVerifying())
                {
                    return new FakeAuthenticationManager();
                }

                ThrowIf.Argument.IsNull(HttpContext.Current, "HttpContext.Current");
                return container.GetInstance<IOwinContext>().Authentication;
            }, Lifestyle.Scoped);

            container.Register(() =>
            {
                if (container.IsVerifying())
                {
                    return new FakeOwinContext();
                }

                ThrowIf.Argument.IsNull(HttpContext.Current, "HttpContext.Current");
                return HttpContext.Current.GetOwinContext();
            }, Lifestyle.Scoped);
        }
    }
}
