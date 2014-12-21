﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Domain.Contracts;
using CH.Domain.Contracts.Identity;
using CH.Domain.Contracts.Logging;
using CH.Domain.Contracts.Queries;
using CH.Domain.Identity;
using CH.Domain.Models.Logging;
using CH.Domain.Services.Queries;
using CH.Website.Models.Account;
using TOTD.Utility.ExceptionHelpers;

namespace CH.Website.Services.QueryHandlers
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