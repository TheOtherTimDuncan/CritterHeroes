using System;
using System.Web.Mvc;
using System.Web.Routing;
using CritterHeroes.Web.Domain.Contracts;
using CritterHeroes.Web.Shared.VersionedStatics;
using SimpleInjector.Integration.Web.Mvc;

namespace CritterHeroes.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var container = DIConfig.ConfigureDependencyContainer();
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

            ValidationConfig.ConfigureValidation(container);
            VersionedStatics.Configure(container.GetInstance<IFileSystem>(), container.GetInstance<IHttpContext>());

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ViewEngineConfig.ConfigureViewEngines(ViewEngines.Engines);
        }
    }
}
