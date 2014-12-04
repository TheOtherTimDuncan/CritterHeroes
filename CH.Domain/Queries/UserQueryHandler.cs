using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Domain.Contracts.Identity;
using CH.Domain.Contracts.Queries;
using CH.Domain.Identity;
using TOTD.Utility.ExceptionHelpers;

namespace CH.Domain.Queries
{
    public class UserQueryHandler : IQueryHandler<UserQuery, IdentityUser>
    {
        private IApplicationUserStore _userStore;

        public UserQueryHandler(IApplicationUserStore userStore)
        {
            this._userStore = userStore;
        }

        public async Task<IdentityUser> Retrieve(UserQuery query)
        {
            ThrowIf.Argument.IsNull(query, "query");
            return await _userStore.FindByNameAsync(query.Username);
        }
    }
}
