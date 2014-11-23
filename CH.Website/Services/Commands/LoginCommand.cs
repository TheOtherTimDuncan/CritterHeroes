using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CH.Domain.Commands;
using CH.Domain.Contracts;
using CH.Domain.Contracts.Commands;
using CH.Domain.Contracts.Identity;
using CH.Domain.Contracts.Logging;
using CH.Domain.Models.Logging;
using CH.Website.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace CH.Website.Services.Commands
{
    public class LoginCommand : ICommandHandler<LoginModel>
    {
        private IApplicationSignInManager _signinManager;
        private IUserLogger _userLogger;

        public LoginCommand(IApplicationSignInManager signinManager, IUserLogger userLogger)
        {
            this._signinManager = signinManager;
            this._userLogger = userLogger;
        }

        public async Task<CommandResult> Execute(LoginModel command)
        {
            SignInStatus result = await _signinManager.PasswordSignInAsync(command.Username, command.Password, isPersistent: false, shouldLockout: false);

            if (result == SignInStatus.Success)
            {
                await _userLogger.LogAction(UserActions.PasswordLoginSuccess, command.Username);
                return CommandResult.Success();
            }
            else
            {
                await _userLogger.LogAction(UserActions.PasswordLoginFailure, command.Username);
                return CommandResult.Failed("", "The username or password that you entered was incorrect. Please try again.");
            }
        }
    }
}