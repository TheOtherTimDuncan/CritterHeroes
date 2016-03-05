using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Data.Models.Identity;
using CritterHeroes.Web.Models.LogEvents;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace CritterHeroes.Web.Areas.Account.CommandHandlers
{
    public class ResetPasswordCommandHandler : IAsyncCommandHandler<ResetPasswordModel>
    {
        private IAppUserManager _userManager;
        private IAppSignInManager _signinManager;
        private IAppLogger _logger;
        private IEmailService _emailService;

        public ResetPasswordCommandHandler(IAppLogger logger, IAppSignInManager signinManager, IAppUserManager userManager, IEmailService emailService)
        {
            this._userManager = userManager;
            this._signinManager = signinManager;
            this._logger = logger;
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
                        _logger.LogEvent(UserLogEvent.LogAction("Reset password succeeded for {Email}", command.Email));

                        // Let the view know we succeeded
                        command.IsSuccess = true;

                        return CommandResult.Success();
                    }
                    else
                    {
                        _logger.LogEvent(UserLogEvent.LogError("Reset password succeeded but login failed for {Email}", command.Email));
                    }
                }
                else
                {
                    _logger.LogEvent(UserLogEvent.LogError("Reset password failed for {Email} using code {Code}", identityResult.Errors, command.Email, command.Code));
                }
            }
            else
            {
                _logger.LogEvent(UserLogEvent.LogError("Reset password failed for {Email} using code {Code} - user not found", command.Email, command.Code));
            }

            return CommandResult.Failed("There was an error resetting your password. Please try again.");
        }
    }
}
