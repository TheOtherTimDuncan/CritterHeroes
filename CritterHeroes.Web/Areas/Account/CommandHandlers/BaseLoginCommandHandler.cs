using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Models.Logging;
using Microsoft.AspNet.Identity.Owin;

namespace CritterHeroes.Web.Areas.Account.CommandHandlers
{
    public abstract class BaseLoginCommandHandler<TParameter> : IAsyncUserCommandHandler<TParameter>
        where TParameter : LoginModel
    {
        private IApplicationSignInManager _signinManager;

        public BaseLoginCommandHandler(IApplicationSignInManager signinManager)
        {
            this._signinManager = signinManager;
        }

        public abstract UserActions SuccessUserAction
        {
            get;
        }

        public abstract UserActions FailedUserAction
        {
            get;
        }

        public abstract Task<CommandResult> ExecuteAsync(TParameter command);

        public virtual async Task<CommandResult> Login(TParameter command)
        {
            SignInStatus result = await _signinManager.PasswordSignInAsync(command.Username, command.Password, isPersistent: false, shouldLockout: false);

            if (result == SignInStatus.Success)
            {
                return CommandResult.Success();
            }
            else
            {
                return CommandResult.Failed("", "The username or password that you entered was incorrect. Please try again.");
            }

        }
    }
}