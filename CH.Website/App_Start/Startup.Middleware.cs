using System;
using CH.Dependency;
using CH.Domain.Contracts;
using CH.Website.Middleware;
using Owin;

namespace CH.Website
{
    public partial class Startup
    {
        public void ConfigureMiddleware(IAppBuilder app)
        {
            app.UseOrganizationContext(DependencyContainer.Using<IAppDependencyResolver>());
        }
    }
}