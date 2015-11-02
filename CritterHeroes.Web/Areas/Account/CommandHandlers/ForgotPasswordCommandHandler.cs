using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Areas.Common.ActionExtensions;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Models.Identity;
using CritterHeroes.Web.Models.Logging;

namespace CritterHeroes.Web.Areas.Account.CommandHandlers
{
    public class ForgotPasswordCommandHandler : IAsyncCommandHandler<ForgotPasswordModel>
    {
        private IUserLogger _userLogger;
        private IAppUserManager _appUserManager;
        private IEmailService _emailService;
        private IUrlGenerator _urlGenerator;
        private IStateManager<OrganizationContext> _stateManager;
        private IOrganizationLogoService _logoService;

        public ForgotPasswordCommandHandler(IUserLogger userLogger, IAppUserManager userManager, IEmailService emailService, IUrlGenerator urlGenerator, IStateManager<OrganizationContext> stateManager, IOrganizationLogoService logoService)
        {
            this._userLogger = userLogger;
            this._appUserManager = userManager;
            this._emailService = emailService;
            this._urlGenerator = urlGenerator;
            this._stateManager = stateManager;
            this._logoService = logoService;
        }

        public async Task<CommandResult> ExecuteAsync(ForgotPasswordModel command)
        {
            AppUser user = await _appUserManager.FindByEmailAsync(command.ResetPasswordEmail);

            OrganizationContext organizationContext = _stateManager.GetContext();

            if (user == null)
            {
                // We don't want to reveal whether or not the username or email address are valid
                // so if the user isn't found send an email to the entered email address and just return success
                string homeUrl = _urlGenerator.GenerateAbsoluteHomeUrl();
                ResetPasswordAttemptEmailCommand emailAttempt = new ResetPasswordAttemptEmailCommand(command.ResetPasswordEmail, homeUrl, _logoService.GetLogoUrl(), organizationContext.FullName);
                await _emailService.SendEmailAsync(emailAttempt);
                await _userLogger.LogActionAsync(UserActions.ForgotPasswordFailure, command.ResetPasswordEmail);
                return CommandResult.Success();
            }

            ResetPasswordEmailCommand emailCommand = new ResetPasswordEmailCommand(user.Email)
            {
                TokenLifespan = _appUserManager.TokenLifespan
            };

            emailCommand.Token = await _appUserManager.GeneratePasswordResetTokenAsync(user.Id);
            emailCommand.Url = _urlGenerator.GenerateResetPasswordAbsoluteUrl(emailCommand.Token);

            await _emailService.SendEmailAsync(emailCommand);
            await _userLogger.LogActionAsync(UserActions.ForgotPasswordSuccess, user.Email);

            return CommandResult.Success();
        }
    }
}
