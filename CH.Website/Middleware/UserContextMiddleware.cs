using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CH.Domain.Contracts.Queries;
using CH.Domain.Identity;
using CH.Domain.Services.Queries;
using CH.Domain.StateManagement;
using Microsoft.Owin;
using Owin;
using TOTD.Utility.ExceptionHelpers;
using TOTD.Utility.StringHelpers;

namespace CH.Website.Middleware
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
                IAsyncQueryHandler<UserIDQuery, UserContext> queryHandler = _dependencyResolver.GetService<IAsyncQueryHandler<UserIDQuery, UserContext>>();

                UserContext userContext = await queryHandler.RetrieveAsync(new UserIDQuery()
                {
                    UserID = context.Request.User.GetUserID()
                });

                context.SetUserContext(userContext);
            }

            await Next.Invoke(context);
        }
    }
}