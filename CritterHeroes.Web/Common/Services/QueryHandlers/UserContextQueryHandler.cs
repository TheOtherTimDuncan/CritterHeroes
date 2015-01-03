using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Common.StateManagement;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Common.Services.Queries
{
    public class UserContextQueryHandler : IAsyncQueryHandler<UserIDQuery, UserContext>
    {
        private IApplicationUserStore _userStore;
        private IStateManager<UserContext> _stateManager;

        public UserContextQueryHandler(IApplicationUserStore userStore, IStateManager<UserContext> stateManager)
        {
            this._userStore = userStore;
            this._stateManager = stateManager;
        }

        public async Task<UserContext> RetrieveAsync(UserIDQuery query)
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
