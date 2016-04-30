using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Data.Models.Identity;
using CritterHeroes.Web.Features.Account.Models;
using Microsoft.AspNet.Identity;

namespace CritterHeroes.Web.Features.Account.Commands
{
    public class EditProfileCommandHandler : IAsyncCommandHandler<EditProfileModel>
    {
        private IAppUserManager _userManager;
        private IHttpUser _httpUser;
        private IStateManager<UserContext> _userContextManager;

        public EditProfileCommandHandler(IAppUserManager userManager, IHttpUser httpUser, IStateManager<UserContext> userContextManager)
        {
            this._userManager = userManager;
            this._httpUser = httpUser;
            this._userContextManager = userContextManager;
        }

        public async Task<CommandResult> ExecuteAsync(EditProfileModel command)
        {
            AppUser user = await _userManager.FindByNameAsync(_httpUser.Username);
            user.Person.FirstName = command.FirstName;
            user.Person.LastName = command.LastName;

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
