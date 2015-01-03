using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Common.Services.Commands;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Logging;

namespace CritterHeroes.Web.Areas.Account.Handlers
{
    public class LoginCommandHandler : BaseLoginCommandHandler<LoginModel>
    {
        public LoginCommandHandler(IApplicationSignInManager signinManager, IUserLogger userLogger)
            : base(signinManager, userLogger)
        {
        }

        public override async Task<CommandResult> ExecuteAsync(LoginModel command)
        {
            return await Login(command);
        }
    }
}