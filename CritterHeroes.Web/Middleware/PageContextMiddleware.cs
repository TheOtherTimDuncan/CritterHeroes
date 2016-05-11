using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Shared.OwinExtensions;
using CritterHeroes.Web.Shared.StateManagement;
using Microsoft.Owin;
using Owin;
using TOTD.Utility.ExceptionHelpers;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.Middleware
{
    public static class PageContextMiddlewareExtensions
    {
        public static void UsePageContext(this IAppBuilder builder, IDependencyResolver dependencyResolver)
        {
            builder.Use<PageContextMiddleware>(dependencyResolver);
        }
    }

    public class PageContextMiddleware : OwinMiddleware
    {
        private IDependencyResolver _dependencyResolver;

        public PageContextMiddleware(OwinMiddleware next, IDependencyResolver dependencyResolver)
            : base(next)
        {
            ThrowIf.Argument.IsNull(dependencyResolver, "dependencyResolver");
            this._dependencyResolver = dependencyResolver;
        }

        public override async Task Invoke(IOwinContext context)
        {
            string referrer = context.Request.GetReferrer();

            if (!referrer.IsNullOrEmpty())
            {
                Uri uriReferrer = new Uri(referrer);

                // Only preserve our own urls not someone else's
                if (uriReferrer.Host.Equals(context.Request.Uri.Host))
                {
                    // Only preserve the previous url if the current url is different
                    // Ignore page refresh or post backs
                    if (!uriReferrer.AbsolutePath.Equals(context.Request.Uri.AbsolutePath, StringComparison.OrdinalIgnoreCase))
                    {
                        IPageContextService servicePageContext = _dependencyResolver.GetService<IPageContextService>();

                        // First check to see if it's already been cached in the OwinContext
                        PageContext pageContext = servicePageContext.GetPageContext();
                        if (pageContext == null)
                        {
                            // If it's not there let's create it
                            pageContext = new PageContext
                            {
                                PreviousPath = uriReferrer.LocalPath
                            };

                            // Cache the result in the response for the next request
                            IStateManager<PageContext> stateManager = _dependencyResolver.GetService<IStateManager<PageContext>>();
                            stateManager.SaveContext(pageContext);

                            // Cache it in the OwinContext for this request
                            servicePageContext.SavePageContext(pageContext);
                        }
                    }
                }
            }

            await Next.Invoke(context);
        }
    }
}
