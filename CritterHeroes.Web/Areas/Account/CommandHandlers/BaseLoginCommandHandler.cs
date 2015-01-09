using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.Notifications;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Notifications;
using CritterHeroes.Web.Models.Logging;
using Microsoft.AspNet.Identity.Owin;

namespace CritterHeroes.Web.Areas.Account.CommandHandlers
{
    public abstract class BaseLoginCommandHandler<TParameter> : IAsyncCommandHandler<TParameter>
        where TParameter : LoginModel
    {
        private IApplicationSignInManager _signinManager;
        private INotificationPublisher _notificationPublisher;

        public BaseLoginCommandHandler(INotificationPublisher notificationPublisher, IApplicationSignInManager signinManager)
        {
            this._signinManager = signinManager;
            this._notificationPublisher = notificationPublisher;
        }

        public abstract Task<CommandResult> ExecuteAsync(TParameter command);

        public virtual async Task<CommandResult> Login(TParameter command)
        {
            SignInStatus result = await _signinManager.PasswordSignInAsync(command.Username, command.Password, isPersistent: false, shouldLockout: false);

            if (result == SignInStatus.Success)
            {
                await _notificationPublisher.PublishAsync(new UserActionNotification(UserActions.PasswordLoginSuccess, command.Username));
                return CommandResult.Success();
            }
            else
            {
                await _notificationPublisher.PublishAsync(new UserActionNotification(UserActions.PasswordLoginFailure, command.Username));
                return CommandResult.Failed("", "The username or password that you entered was incorrect. Please try again.");
            }
        }
    }
}