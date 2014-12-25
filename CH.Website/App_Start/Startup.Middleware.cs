using System;
using System.Web.Mvc;
using CH.Website.Dependency;
using CH.Website.Middleware;
using Owin;

namespace CH.Website
{
    public partial class Startup
    {
        public void ConfigureMiddleware(IAppBuilder app)
        {
            app.UseOrganizationContext(DependencyResolver.Current);
            app.UseUserContext(DependencyResolver.Current);
        }
    }
}