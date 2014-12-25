using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CH.Domain.Contracts.Configuration;
using CH.Domain.Contracts.Queries;
using CH.Domain.Services.Queries;
using CH.Domain.StateManagement;
using Microsoft.Owin;
using Owin;
using TOTD.Utility.ExceptionHelpers;

namespace CH.Website.Middleware
{
    public static class OrganizationContextMiddlewareExtensions
    {
        private const string _key = "CritterHeroes.Organization";

        public static void UseOrganizationContext(this IAppBuilder builder, IDependencyResolver dependencyResolver)
        {
            builder.Use<OrganizationContextMiddleware>(dependencyResolver);
        }

        public static OrganizationContext GetOrganizationContext(this IOwinContext owinContext)
        {
            return owinContext.Get<OrganizationContext>(_key);
        }

        public static void SetOrganizationContext(this IOwinContext owinContext, OrganizationContext organizationContext)
        {
            owinContext.Set(_key, organizationContext);
        }
    }

    public class OrganizationContextMiddleware : OwinMiddleware
    {
        private IDependencyResolver _dependencyResolver;

        public OrganizationContextMiddleware(OwinMiddleware next, IDependencyResolver dependencyResolver)
            : base(next)
        {
            ThrowIf.Argument.IsNull(dependencyResolver, "dependencyResolver");
            this._dependencyResolver = dependencyResolver;
        }

        public override async Task Invoke(IOwinContext context)
        {
            IAsyncQueryHandler<OrganizationQuery, OrganizationContext> queryHandler = _dependencyResolver.GetService<IAsyncQueryHandler<OrganizationQuery, OrganizationContext>>();
            IAppConfiguration appConfiguration = _dependencyResolver.GetService<IAppConfiguration>();

            OrganizationContext organizationContext = await queryHandler.RetrieveAsync(new OrganizationQuery()
            {
                OrganizationID = appConfiguration.OrganizationID
            });

            context.SetOrganizationContext(organizationContext);

            await Next.Invoke(context);
        }
    }
}