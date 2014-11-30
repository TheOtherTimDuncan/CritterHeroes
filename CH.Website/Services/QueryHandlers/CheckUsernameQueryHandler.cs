using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Domain.Contracts;
using CH.Domain.Contracts.Identity;
using CH.Domain.Contracts.Logging;
using CH.Domain.Contracts.Queries;
using CH.Domain.Identity;
using CH.Domain.Models.Logging;
using CH.Website.Models;
using CH.Website.Services.Queries;

namespace CH.Website.Services.QueryHandlers
{
    public class CheckUsernameQueryHandler : IQueryHandler<CheckUsernameQuery, CheckUsernameResult>
    {
        public IUserLogger _userLogger;
        public IApplicationUserManager _userManager;
        public IHttpContext _httpContext;

        public CheckUsernameQueryHandler(IUserLogger userLogger, IApplicationUserManager userManager, IHttpContext httpContext)
        {
            this._userLogger = userLogger;
            this._userManager = userManager;
            this._httpContext = httpContext;
        }

        public async Task<CheckUsernameResult> Retrieve(CheckUsernameQuery query)
        {
            var logData = new
            {
                Request = query.Username,
                RequestFrom = _httpContext.Request.UrlReferrer.AbsoluteUri,
            };

            await _userLogger.LogAction(UserActions.DuplicateUsernameCheck, _httpContext.User.Identity.Name, logData);
            IdentityUser user = await _userManager.FindByNameAsync(query.Username);
            return new CheckUsernameResult()
            {
                UserExists = (user != null)
            };
        }
    }
}