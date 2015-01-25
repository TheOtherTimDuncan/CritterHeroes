using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Common.OwinExtensions;
using CritterHeroes.Web.Common.Queries;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Models.Logging;
using Microsoft.Owin;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Areas.Account.QueryHandlers
{
    public class CheckUsernameQueryHandler : IAsyncQueryHandler<UsernameQuery, CheckUsernameResult>
    {
        public IUserLogger _userLogger;
        public IApplicationUserStore _userStore;
        private IOwinContext _owinContext;

        public CheckUsernameQueryHandler(IUserLogger userLogger, IApplicationUserStore userStore, IOwinContext owinContext)
        {
            this._userLogger = userLogger;
            this._userStore = userStore;
            this._owinContext = owinContext;
        }

        public async Task<CheckUsernameResult> RetrieveAsync(UsernameQuery query)
        {
            ThrowIf.Argument.IsNull(query, "query");

            var logData = new
            {
                Request = query.Username,
                RequestFrom = _owinContext.Request.GetReferrer(),
            };

            await _userLogger.LogActionAsync(UserActions.DuplicateUsernameCheck, _owinContext.Request.User.Identity.Name, logData);
            IdentityUser user = await _userStore.FindByNameAsync(query.Username);
            return new CheckUsernameResult()
            {
                UserExists = (user != null)
            };
        }
    }
}