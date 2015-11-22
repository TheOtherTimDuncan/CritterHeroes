using System;
using System.Web.Mvc;
using System.Web.Routing;
using CritterHeroes.Web.Common.VersionedStatics;
using CritterHeroes.Web.Contracts;

namespace CritterHeroes.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var container = DIConfig.ConfigureDependencyContainer();

            ValidationConfig.ConfigureValidation(container);
            VersionedStatics.Configure(container.GetInstance<IFileSystem>(), container.GetInstance<IHttpContext>());

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            DefaultHtmlConventions.AddHtmlConventions();

            ViewEngineConfig.ConfigureViewEngines(ViewEngines.Engines);
        }
    }
}
