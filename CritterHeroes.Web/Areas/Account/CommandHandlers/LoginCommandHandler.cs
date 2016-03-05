using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Events;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Models.LogEvents;
using Microsoft.AspNet.Identity.Owin;

namespace CritterHeroes.Web.Areas.Account.CommandHandlers
{
    public class LoginCommandHandler : IAsyncCommandHandler<LoginModel>
    {
        private IAppSignInManager _signinManager;
        private IAppEventPublisher _publisher;

        public LoginCommandHandler(IAppEventPublisher publisher, IAppSignInManager signinManager)
        {
            this._signinManager = signinManager;
            this._publisher = publisher;
        }

        public async Task<CommandResult> ExecuteAsync(LoginModel command)
        {
            SignInStatus result = await _signinManager.PasswordSignInAsync(command.Email, command.Password);

            if (result == SignInStatus.Success)
            {
                _publisher.Publish(UserLogEvent.Action("{Email} successfully logged in", command.Email));
                return CommandResult.Success();
            }
            else
            {
                _publisher.Publish(UserLogEvent.Error("{Email} failed login because {SignInStatus}", command.Email, result));
                return CommandResult.Failed("The username or password that you entered was incorrect. Please try again.");
            }
        }
    }
}
