using System;
using CH.Domain.Contracts;
using CH.Website.Dependency;
using CH.Website.Middleware;
using Owin;

namespace CH.Website
{
    public partial class Startup
    {
        public void ConfigureMiddleware(IAppBuilder app)
        {
            IAppDependencyResolver dependencyResolver = DependencyContainer.Using<IAppDependencyResolver>();

            app.UseOrganizationContext(dependencyResolver);
            app.UseUserContext(dependencyResolver);
        }
    }
}