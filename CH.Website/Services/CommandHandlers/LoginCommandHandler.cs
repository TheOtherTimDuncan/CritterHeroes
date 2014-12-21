using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Domain.Contracts.Identity;
using CH.Domain.Contracts.Logging;
using CH.Domain.Services.Commands;
using CH.Website.Models.Account;

namespace CH.Website.Services.CommandHandlers
{
    public class LoginCommandHandler : BaseLoginCommandHandler<LoginModel, CommandResult>
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