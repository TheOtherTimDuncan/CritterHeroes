using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Models;
using Microsoft.Owin;
using Owin;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Middleware
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
            // First check to see if it's already been cached in the OwinContext
            OrganizationContext organizationContext = context.GetOrganizationContext();
            if (organizationContext == null)
            {
                // Next check the request
                IStateManager<OrganizationContext> stateManager = _dependencyResolver.GetService<IStateManager<OrganizationContext>>();
                organizationContext = stateManager.GetContext();
                if (organizationContext == null)
                {
                    // It must not exist at all yet so let's create it
                    IStorageContext<Organization> storageContext = _dependencyResolver.GetService<IStorageContext<Organization>>();
                    IAppConfiguration appConfiguration = _dependencyResolver.GetService<IAppConfiguration>();
                    Organization organization = await storageContext.GetAsync(appConfiguration.OrganizationID.ToString());
                    organizationContext = OrganizationContext.FromOrganization(organization);

                    // Cache the result in the response for the next request
                    stateManager.SaveContext(organizationContext);
                }

                // Cache it in the OwinContext for this request
                context.SetOrganizationContext(organizationContext);
            }

            await Next.Invoke(context);
        }
    }
}