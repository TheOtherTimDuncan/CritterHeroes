using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Extensions;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.Shared.StateManagement;
using Microsoft.Owin;
using Owin;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Middleware
{
    public static class OrganizationContextMiddlewareExtensions
    {
        public static void UseOrganizationContext(this IAppBuilder builder, IDependencyResolver dependencyResolver)
        {
            builder.Use<OrganizationContextMiddleware>(dependencyResolver);
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
            // Check to see if the cookie already exists
            IStateManager<OrganizationContext> stateManager = _dependencyResolver.GetService<IStateManager<OrganizationContext>>();
            OrganizationContext organizationContext = stateManager.GetContext();

            if (organizationContext == null)
            {
                // It must not exist so let's create it
                ISqlQueryStorageContext<Organization> storageContext = _dependencyResolver.GetService<ISqlQueryStorageContext<Organization>>();
                IAppConfiguration appConfiguration = _dependencyResolver.GetService<IAppConfiguration>();
                Organization organization = await storageContext.Entities.FindByIDAsync(appConfiguration.OrganizationID);
                organizationContext = OrganizationContext.FromOrganization(organization);

                // Cache the result in the response for the next request
                stateManager.SaveContext(organizationContext);
            }

            await Next.Invoke(context);
        }
    }
}
