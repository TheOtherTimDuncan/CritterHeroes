using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Notifications;

namespace CritterHeroes.Web.Areas.Account.CommandHandlers
{
    public class LoginCommandHandler : BaseLoginCommandHandler<LoginModel>
    {
        public LoginCommandHandler(INotificationPublisher notificationPublisher, IApplicationSignInManager signinManager)
            : base(notificationPublisher, signinManager)
        {
        }

        public override async Task<CommandResult> ExecuteAsync(LoginModel command)
        {
            return await Login(command);
        }
    }
}