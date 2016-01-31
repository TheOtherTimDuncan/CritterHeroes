using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Logging;
using Microsoft.AspNet.Identity.Owin;

namespace CritterHeroes.Web.Areas.Account.CommandHandlers
{
    public class LoginCommandHandler : IAsyncCommandHandler<LoginModel>
    {
        private IAppSignInManager _signinManager;
        private IUserLogger _userLogger;

        public LoginCommandHandler(IUserLogger userLogger, IAppSignInManager signinManager)
        {
            this._signinManager = signinManager;
            this._userLogger = userLogger;
        }

        public async Task<CommandResult> ExecuteAsync(LoginModel command)
        {
            SignInStatus result = await _signinManager.PasswordSignInAsync(command.Email, command.Password);

            if (result == SignInStatus.Success)
            {
                _userLogger.LogAction("{Email} successfully logged in", command.Email);
                return CommandResult.Success();
            }
            else
            {
                _userLogger.LogError("{Email} failed login because {SignInStatus}", command.Email, result);
                return CommandResult.Failed("The username or password that you entered was incorrect. Please try again.");
            }
        }
    }
}
