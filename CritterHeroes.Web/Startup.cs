using System;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(CritterHeroes.Web.Startup))]
namespace CritterHeroes.Web
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
