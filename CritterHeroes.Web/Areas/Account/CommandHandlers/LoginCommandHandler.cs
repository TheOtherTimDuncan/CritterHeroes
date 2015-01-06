using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Models.Logging;

namespace CritterHeroes.Web.Areas.Account.CommandHandlers
{
    public class LoginCommandHandler : BaseLoginCommandHandler<LoginModel>
    {
        public LoginCommandHandler(IApplicationSignInManager signinManager)
            : base(signinManager)
        {
        }

        public override async Task<CommandResult> ExecuteAsync(LoginModel command)
        {
            return await Login(command);
        }

        public override UserActions SuccessUserAction
        {
            get
            {
                return UserActions.PasswordLoginSuccess;
            }
        }

        public override UserActions FailedUserAction
        {
            get
            {
                return UserActions.PasswordLoginFailure;
            }
        }
    }
}