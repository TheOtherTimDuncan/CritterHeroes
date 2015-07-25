using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.StateManagement;
using Microsoft.AspNet.Identity;

namespace CritterHeroes.Web.Areas.Account.CommandHandlers
{
    public class EditProfileCommandHandler : IAsyncCommandHandler<EditProfileModel>
    {
        private IAzureAppUserManager _userManager;
        private IHttpUser _httpUser;
        private IStateManager<UserContext> _userContextManager;

        public EditProfileCommandHandler(IAzureAppUserManager userManager, IHttpUser httpUser, IStateManager<UserContext> userContextManager)
        {
            this._userManager = userManager;
            this._httpUser = httpUser;
            this._userContextManager = userContextManager;
        }

        public async Task<CommandResult> ExecuteAsync(EditProfileModel command)
        {
            AzureAppUser user = await _userManager.FindByIdAsync(_httpUser.UserID);
            user.FirstName = command.FirstName;
            user.LastName = command.LastName;

            IdentityResult identityResult = await _userManager.UpdateAsync(user);
            if (!identityResult.Succeeded)
            {
                return CommandResult.Failed(identityResult.Errors);
            }

            UserContext userContext = UserContext.FromUser(user);
            _userContextManager.SaveContext(userContext);

            return CommandResult.Success();
        }
    }
}