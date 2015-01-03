using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Common.Services.Queries;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Models.Logging;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Areas.Account.Handlers
{
    public class CheckUsernameQueryHandler : IAsyncQueryHandler<UsernameQuery, CheckUsernameResult>
    {
        public IUserLogger _userLogger;
        public IApplicationUserStore _userStore;
        public IHttpContext _httpContext;

        public CheckUsernameQueryHandler(IUserLogger userLogger, IApplicationUserStore userStore, IHttpContext httpContext)
        {
            this._userLogger = userLogger;
            this._userStore = userStore;
            this._httpContext = httpContext;
        }

        public async Task<CheckUsernameResult> RetrieveAsync(UsernameQuery query)
        {
            ThrowIf.Argument.IsNull(query, "query");

            var logData = new
            {
                Request = query.Username,
                RequestFrom = _httpContext.Request.UrlReferrer.AbsoluteUri,
            };

            await _userLogger.LogActionAsync(UserActions.DuplicateUsernameCheck, _httpContext.User.Identity.Name, logData);
            IdentityUser user = await _userStore.FindByNameAsync(query.Username);
            return new CheckUsernameResult()
            {
                UserExists = (user != null)
            };
        }
    }
}