using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Data.Models.Identity;
using CritterHeroes.Web.Domain.Contracts.Commands;
using CritterHeroes.Web.Domain.Contracts.Email;
using CritterHeroes.Web.Domain.Contracts.Events;
using CritterHeroes.Web.Domain.Contracts.Identity;
using CritterHeroes.Web.Features.Account.Models;
using CritterHeroes.Web.Models.LogEvents;
using CritterHeroes.Web.Shared.Commands;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace CritterHeroes.Web.Features.Account.Commands
{
    public class ResetPasswordCommandHandler : IAsyncCommandHandler<ResetPasswordModel>
    {
        private IAppUserManager _userManager;
        private IAppSignInManager _signinManager;
        private IAppEventPublisher _publisher;
        private IEmailService<ResetPasswordNotificationEmailCommand> _emailService;

        public ResetPasswordCommandHandler(IAppEventPublisher publisher, IAppSignInManager signinManager, IAppUserManager userManager, IEmailService<ResetPasswordNotificationEmailCommand> emailService)
        {
            this._userManager = userManager;
            this._signinManager = signinManager;
            this._publisher = publisher;
            this._emailService = emailService;
        }

        public async Task<CommandResult> ExecuteAsync(ResetPasswordModel command)
        {
            AppUser identityUser = await _userManager.FindByEmailAsync(command.Email);
            if (identityUser != null)
            {
                IdentityResult identityResult = await _userManager.ResetPasswordAsync(identityUser.Id, command.Code, command.Password);
                if (identityResult.Succeeded)
                {
                    SignInStatus loginResult = await _signinManager.PasswordSignInAsync(command.Email, command.Password);
                    if (loginResult == SignInStatus.Success)
                    {
                        ResetPasswordNotificationEmailCommand emailCommand = new ResetPasswordNotificationEmailCommand(identityUser.Email);
                        await _emailService.SendEmailAsync(emailCommand);
                        _publisher.Publish(UserLogEvent.Action("Reset password succeeded for {Email}", command.Email));

                        // Let the view know we succeeded
                        command.IsSuccess = true;

                        return CommandResult.Success();
                    }
                    else
                    {
                        _publisher.Publish(UserLogEvent.Error("Reset password succeeded but login failed for {Email}", command.Email));
                    }
                }
                else
                {
                    _publisher.Publish(UserLogEvent.Error("Reset password failed for {Email} using code {Code}", identityResult.Errors, command.Email, command.Code));
                }
            }
            else
            {
                _publisher.Publish(UserLogEvent.Error("Reset password failed for {Email} using code {Code} - user not found", command.Email, command.Code));
            }

            return CommandResult.Failed("There was an error resetting your password. Please try again.");
        }
    }
}
