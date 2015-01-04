using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Identity;
using Microsoft.Owin;
using Owin;
using TOTD.Utility.ExceptionHelpers;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.Middleware
{
    public static class UserContextMiddlewareExtensions
    {
        private const string _key = "CritterHeroes.User";

        public static void UseUserContext(this IAppBuilder builder, IDependencyResolver dependencyResolver)
        {
            builder.Use<UserContextMiddleware>(dependencyResolver);
        }

        public static UserContext GetUserContext(this IOwinContext owinContext)
        {
            return owinContext.Get<UserContext>(_key);
        }

        public static void SetUserContext(this IOwinContext owinContext, UserContext userContext)
        {
            owinContext.Set(_key, userContext);
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
                // First check to see if it is already cached in the OwinContext
                UserContext userContext = context.GetUserContext();
                if (userContext == null)
                {
                    // Next check the request
                    IStateManager<UserContext> stateManager = _dependencyResolver.GetService<IStateManager<UserContext>>();
                    userContext = stateManager.GetContext();
                    if (userContext == null)
                    {
                        // It must not exist at all so let's create it
                        IApplicationUserStore userStore = _dependencyResolver.GetService<IApplicationUserStore>();
                        IdentityUser user = await userStore.FindByIdAsync(context.Request.User.GetUserID());
                        userContext = UserContext.FromUser(user);

                        // Cache the result in the response for the next request
                        stateManager.SaveContext(userContext);
                    }

                    // Cache it in the OwinContext for this request
                    context.SetUserContext(userContext);
                }
            }

            await Next.Invoke(context);
        }
    }
}