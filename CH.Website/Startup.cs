using System;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(CH.Website.Startup))]
namespace CH.Website
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            ConfigureMiddleware(app);
        }
    }
}
