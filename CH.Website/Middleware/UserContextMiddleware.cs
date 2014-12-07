using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Domain.Contracts;
using CH.Domain.Contracts.Configuration;
using CH.Domain.Services.Queries;
using CH.Domain.StateManagement;
using Microsoft.Owin;
using Owin;
using TOTD.Utility.ExceptionHelpers;

namespace CH.Website.Middleware
{
    public static class UserContextMiddlewareExtensions
    {
        private const string _key = "CritterHeroes.User";

        public static void UseUserContext(this IAppBuilder builder, IAppDependencyResolver dependencyResolver)
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
        private IAppDependencyResolver _dependencyResolver;

        public UserContextMiddleware(OwinMiddleware next, IAppDependencyResolver dependencyResolver)
            : base(next)
        {
            ThrowIf.Argument.IsNull(dependencyResolver, "dependencyResolver");
            this._dependencyResolver = dependencyResolver;
        }

        public override async Task Invoke(IOwinContext context)
        {
            if (context.Request.User.Identity.IsAuthenticated)
            {
                UserContextQueryHandler queryHandler = _dependencyResolver.Resolve<UserContextQueryHandler>();

                UserContext userContext = await queryHandler.Retrieve(new UserQuery()
                {
                    Username = context.Request.User.Identity.Name
                });

                context.SetUserContext(userContext);
            }

            await Next.Invoke(context);
        }
    }
}