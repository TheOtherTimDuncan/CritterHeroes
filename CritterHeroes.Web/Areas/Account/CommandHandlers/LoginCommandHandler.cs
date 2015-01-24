using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Logging;

namespace CritterHeroes.Web.Areas.Account.CommandHandlers
{
    public class LoginCommandHandler : BaseLoginCommandHandler<LoginModel>
    {
        public LoginCommandHandler(IUserLogger userLogger, IApplicationSignInManager signinManager)
            : base(userLogger, signinManager)
        {
        }

        public override async Task<CommandResult> ExecuteAsync(LoginModel command)
        {
            return await Login(command);
        }
    }
}