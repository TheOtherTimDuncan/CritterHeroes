using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Areas.Common.ActionExtensions;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Contracts.Events;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Data.Models.Identity;
using CritterHeroes.Web.Models.LogEvents;

namespace CritterHeroes.Web.Areas.Account.CommandHandlers
{
    public class ForgotPasswordCommandHandler : IAsyncCommandHandler<ForgotPasswordModel>
    {
        private IAppEventPublisher _publisher;
        private IAppUserManager _appUserManager;
        private IEmailService _emailService;
        private IUrlGenerator _urlGenerator;

        public ForgotPasswordCommandHandler(IAppEventPublisher publisher, IAppUserManager userManager, IEmailService emailService, IUrlGenerator urlGenerator)
        {
            this._publisher = publisher;
            this._appUserManager = userManager;
            this._emailService = emailService;
            this._urlGenerator = urlGenerator;
        }

        public async Task<CommandResult> ExecuteAsync(ForgotPasswordModel command)
        {
            AppUser user = await _appUserManager.FindByEmailAsync(command.ResetPasswordEmail);

            if (user == null)
            {
                // We don't want to reveal whether or not the username or email address are valid
                // so if the user isn't found send an email to the entered email address and just return success
                ResetPasswordAttemptEmailCommand emailAttempt = new ResetPasswordAttemptEmailCommand(command.ResetPasswordEmail);
                await _emailService.SendEmailAsync(emailAttempt);
                _publisher.Publish(UserLogEvent.Error("{Email} not found for forgot password", command.ResetPasswordEmail));
                return CommandResult.Success();
            }

            ResetPasswordEmailCommand emailCommand = new ResetPasswordEmailCommand(user.Email);

            emailCommand.EmailData.TokenLifespan = _appUserManager.TokenLifespan;
            emailCommand.EmailData.Token = await _appUserManager.GeneratePasswordResetTokenAsync(user.Id);
            emailCommand.EmailData.UrlReset = _urlGenerator.GenerateResetPasswordAbsoluteUrl(emailCommand.EmailData.Token);

            await _emailService.SendEmailAsync(emailCommand);
            _publisher.Publish(UserLogEvent.Action("{Email} found for forgot password", user.Email));

            return CommandResult.Success();
        }
    }
}
