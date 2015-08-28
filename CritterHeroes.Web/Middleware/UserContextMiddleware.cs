using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Extensions;
using CritterHeroes.Web.Data.Models.Identity;
using Microsoft.Owin;
using Owin;
using TOTD.Utility.ExceptionHelpers;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.Middleware
{
    public static class UserContextMiddlewareExtensions
    {
        public static void UseUserContext(this IAppBuilder builder, IDependencyResolver dependencyResolver)
        {
            builder.Use<UserContextMiddleware>(dependencyResolver);
        }
    }

    public class UserContextMiddleware : OwinMiddleware
    {
        private IDependencyResolver _dependencyResolver;

        public UserContextMiddleware(OwinMiddleware next, IDependencyResolver dependencyResolver)
            : base(next)
        {
            ThrowIf.Argument.IsNull(dependencyResolver, "dependencyResolver");
            this._dependencyResolver = dependencyResolver;
        }

        public override async Task Invoke(IOwinContext context)
        {
            // While logging out user is still authenticated until the request is complete
            // so we have to double-check the request path to avoid re-creating the cookie 
            // while logging out

            if (context.Request.User.Identity.IsAuthenticated && !context.Request.Path.ToString().SafeEquals("/Account/Logout"))
            {
                // Check to see if the cookie already exists
                IStateManager<UserContext> stateManager = _dependencyResolver.GetService<IStateManager<UserContext>>();
                UserContext userContext = stateManager.GetContext();
                if (userContext == null)
                {
                    // It must not exist so let's create it
                    ISqlStorageContext<AppUser> userStorageContext = _dependencyResolver.GetService<ISqlStorageContext<AppUser>>();
                    AppUser user = await userStorageContext.Entities.FindByUsernameAsync(context.Request.User.Identity.Name);
                    userContext = UserContext.FromUser(user);

                    // Cache the result in the response for the next request
                    stateManager.SaveContext(userContext);
                }
            }

            await Next.Invoke(context);
        }
    }
}