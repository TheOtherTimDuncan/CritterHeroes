using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Domain.Contracts.Commands;
using CH.Domain.Contracts.Identity;
using CH.Domain.Contracts.Logging;
using CH.Domain.Models.Logging;
using CH.Domain.Services.Commands;
using CH.Website.Models.Account;
using Microsoft.AspNet.Identity.Owin;

namespace CH.Website.Services.CommandHandlers
{
    public abstract class BaseLoginCommandHandler<TParameter, TResult> : IAsyncCommandHandler<TParameter, TResult>
        where TParameter : LoginModel
        where TResult : ICommandResult
    {
        private IApplicationSignInManager _signinManager;

        public BaseLoginCommandHandler(IApplicationSignInManager signinManager, IUserLogger userLogger)
        {
            this._signinManager = signinManager;
            this.UserLogger = userLogger;
        }

        protected IUserLogger UserLogger
        {
            get;
            private set;
        }

        public abstract Task<TResult> ExecuteAsync(TParameter command);

        public virtual async Task<CommandResult> Login(TParameter command)
        {
            SignInStatus result = await _signinManager.PasswordSignInAsync(command.Username, command.Password, isPersistent: false, shouldLockout: false);

            if (result == SignInStatus.Success)
            {
                await UserLogger.LogActionAsync(UserActions.PasswordLoginSuccess, command.Username);
                return CommandResult.Success();
            }
            else
            {
                await UserLogger.LogActionAsync(UserActions.PasswordLoginFailure, command.Username);
                return CommandResult.Failed("", "The username or password that you entered was incorrect. Please try again.");
            }

        }
    }
}