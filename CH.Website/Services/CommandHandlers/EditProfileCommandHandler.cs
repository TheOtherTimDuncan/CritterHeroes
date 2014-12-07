using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Domain.Services.Commands;
using CH.Domain.Contracts;
using CH.Domain.Contracts.Commands;
using CH.Domain.Contracts.Identity;
using CH.Domain.Contracts.Logging;
using CH.Domain.Identity;
using CH.Domain.Models.Logging;
using CH.Website.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace CH.Website.Services.CommandHandlers
{
    public class EditProfileCommandHandler : ICommandHandler<EditProfileModel>
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

        public async Task<CommandResult> Execute(EditProfileModel command)
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

            IdentityUser user = await _userManager.FindByNameAsync(command.OriginalUsername);
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
                await _userLogger.LogAction(UserActions.UsernameChanged, user.UserName, "Original username: " + command.OriginalUsername);
                _authenticationManager.SignOut();
                _authenticationManager.SignIn(await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie));
            }

            return CommandResult.Success();
        }
    }
}