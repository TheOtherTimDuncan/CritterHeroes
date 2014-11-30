using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Domain.Commands;
using CH.Domain.Contracts;
using CH.Domain.Contracts.Commands;
using CH.Domain.Contracts.Identity;
using CH.Domain.Contracts.Logging;
using CH.Domain.Identity;
using CH.Domain.Models.Logging;
using CH.Website.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace CH.Website.Services.Commands
{
    public class EditProfileCommand:ICommandHandler<EditProfileModel>
    {
        private IHttpContext _httpContext;
        private IApplicationUserManager _userManager;
        private IUserLogger _userLogger;

        public EditProfileCommand(IHttpContext httpContext, IApplicationUserManager userManager, IUserLogger userLogger)
        {
            this._httpContext = httpContext;
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
                IAuthenticationManager authenticationManager = _httpContext.OwinContext.Authentication;
                authenticationManager.SignOut();
                authenticationManager.SignIn(await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie));
            }

            return CommandResult.Success();
        }
    }
}