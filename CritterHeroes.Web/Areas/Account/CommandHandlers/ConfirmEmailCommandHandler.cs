using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Models.Logging;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace CritterHeroes.Web.Areas.Account.CommandHandlers
{
    public class ConfirmEmailCommandHandler : IAsyncCommandHandler<ConfirmEmailModel>
    {
        private IUserLogger _userLogger;
        private IAppUserManager _appUserManager;
        private IAuthenticationManager _authenticationManager;

        public ConfirmEmailCommandHandler(IUserLogger userLogger, IAppUserManager userManager, IAuthenticationManager authenticationManager)
        {
            this._userLogger = userLogger;
            this._appUserManager = userManager;
            this._authenticationManager = authenticationManager;
        }

        public async Task<CommandResult> ExecuteAsync(ConfirmEmailModel command)
        {
            AzureAppUser user = await _appUserManager.FindByEmailAsync(command.Email);

            if (user == null)
            {
                // We don't want to reveal whether or not the email address is valid
                // so if the user isn't found just return failed with no errors
                await _userLogger.LogActionAsync(UserActions.ConfirmEmailFailure, command.Email);
                return CommandResult.Failed("There was an error confirming your email address. Please try again.");
            }

            IdentityResult identityResult = await _appUserManager.ConfirmEmailAsync(user.Id, command.ConfirmationCode);
            if (identityResult.Succeeded)
            {
                // Update the email address
                user.Email = user.NewEmail;
                await _appUserManager.UpdateAsync(user);

                // In case the user is logged in make sure they are logged out so the new email address is used
                _authenticationManager.SignOut();

                // Let the view know we succeeded
                command.IsSuccess = true;

                await _userLogger.LogActionAsync(UserActions.ConfirmEmailSuccess, command.Email);
                return CommandResult.Success();
            }

            await _userLogger.LogActionAsync(UserActions.ConfirmEmailFailure, user.Email, identityResult.Errors);
            return CommandResult.Failed("There was an error confirming your email address. Please try again.");
        }
    }
}