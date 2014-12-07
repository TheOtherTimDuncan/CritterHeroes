using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Domain.Contracts;
using CH.Domain.Contracts.Identity;
using CH.Domain.Contracts.Queries;
using CH.Domain.Identity;
using CH.Domain.StateManagement;
using TOTD.Utility.ExceptionHelpers;

namespace CH.Domain.Services.Queries
{
    public class UserContextQueryHandler : IQueryHandler<UserIDQuery, UserContext>
    {
        private IApplicationUserStore _userStore;
        private IStateManager<UserContext> _stateManager;

        public UserContextQueryHandler(IApplicationUserStore userStore, IStateManager<UserContext> stateManager)
        {
            this._userStore = userStore;
            this._stateManager = stateManager;
        }

        public async Task<UserContext> Retrieve(UserIDQuery query)
        {
            ThrowIf.Argument.IsNull(query, "query");

            UserContext userContext = _stateManager.GetContext();

            if (userContext == null)
            {
                IdentityUser user = await _userStore.FindByIdAsync(query.UserID);
                userContext = UserContext.FromUser(user);
                _stateManager.SaveContext(userContext);
            }

            return userContext;
        }
    }
}
