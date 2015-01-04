using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Models.Logging;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace CritterHeroes.Web.Areas.Account.CommandHandlers
{
    public class EditProfileCommandHandler : IAsyncCommandHandler<EditProfileModel>
    {
        private IAuthenticationManager _authenticationManager;
        private IApplicationUserManager _userManager;
        private IUserLogger _userLogger;

        public EditProfileCommandHandler(IAuthenticationManager httpContext, IApplicationUserManager userManager, IUserLogger userLogger)
        {
            this._authenticationManager = httpContext;
            this._userManager = userManager;
            this._userLogger = userLogger;
        }

        public async Task<CommandResult> ExecuteAsync(EditProfileModel command)
        {
            bool isUsernameChanged = false;

            if (!command.OriginalUsername.Equals(command.Username, StringComparison.InvariantCultureIgnoreCase))
            {
                IdentityUser dupeUser = await _userManager.FindByNameAsync(command.Username);
                if (dupeUser != null)
                {
                    return CommandResult.Failed("", "The username you entered is not available. Please enter a different username.");
                }
                else
                {
                    isUsernameChanged = true;
                }
            }

            IdentityUser user = await _userManager.FindByIdAsync(command.OriginalUsername);
            user.FirstName = command.FirstName;
            user.LastName = command.LastName;

            if (isUsernameChanged)
            {
                user.UserName = command.Username;
            }

            IdentityResult identityResult = await _userManager.UpdateAsync(user);
            if (!identityResult.Succeeded)
            {
                return CommandResult.Failed("", identityResult.Errors);
            }

            if (isUsernameChanged)
            {
                await _userLogger.LogActionAsync(UserActions.UsernameChanged, user.UserName, "Original username: " + command.OriginalUsername);
                _authenticationManager.SignOut();
                _authenticationManager.SignIn(await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie));
            }

            return CommandResult.Success();
        }
    }
}