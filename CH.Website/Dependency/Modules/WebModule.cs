using System;
using System.Web;
using CH.Domain.Contracts;
using CH.Domain.Proxies;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Ninject.Modules;
using TOTD.Utility.ExceptionHelpers;

using CH.Website.Utility;namespace CH.Website.Dependency.Modules
{
    public class WebModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IHttpContext>().To<HttpContextProxy>();

            Bind<IOwinContext>().ToMethod(x =>
            {
                ThrowIf.Argument.IsNull(HttpContext.Current, "HttpContext.Current");
                return HttpContext.Current.GetOwinContext();
            });

            Bind<IAuthenticationManager>().ToMethod(x =>
            {
                ThrowIf.Argument.IsNull(HttpContext.Current, "HttpContext.Current");
                return HttpContext.Current.GetOwinContext().Authentication;
            });

            Bind<IUrlGenerator>().To<UrlGenerator>();
        }
    }
}
