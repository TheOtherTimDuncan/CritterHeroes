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
using Microsoft.AspNet.Identity;

namespace CritterHeroes.Web.Areas.Account.CommandHandlers
{
    public class EditProfileSecureCommandHandler : IAsyncCommandHandler<EditProfileSecureModel>
    {
        private IAppUserManager _userManager;
        private IUserLogger _userLogger;
        private IHttpUser _httpUser;
        private IStateManager<UserContext> _userContextManager;
        private IUrlGenerator _urlGenerator;
        private IEmailService _emailService;
        private IOrganizationLogoService _logoService;
        private IStateManager<OrganizationContext> _stateManager;

        public EditProfileSecureCommandHandler(IAppUserManager userManager, IUserLogger userLogger, IHttpUser httpUser, IStateManager<UserContext> userContextManager, IUrlGenerator urlGenerator, IEmailService emailService, IStateManager<OrganizationContext> stateManager, IOrganizationLogoService logoService)
        {
            this._userManager = userManager;
            this._userLogger = userLogger;
            this._httpUser = httpUser;
            this._userContextManager = userContextManager;
            this._urlGenerator = urlGenerator;
            this._emailService = emailService;
            this._stateManager = stateManager;
            this._logoService = logoService;
        }

        public async Task<CommandResult> ExecuteAsync(EditProfileSecureModel command)
        {
            AppUser user = await _userManager.FindByNameAsync(_httpUser.Username);
            user.Person.NewEmail = command.NewEmail;
            user.EmailConfirmed = false;

            IdentityResult identityResult = await _userManager.UpdateAsync(user);
            if (!identityResult.Succeeded)
            {
                return CommandResult.Failed(identityResult.Errors);
            }

            UserContext userContext = UserContext.FromUser(user);
            _userContextManager.SaveContext(userContext);

            await _userLogger.LogActionAsync(UserActions.EmailChanged, user.Email, "New email: " + command.NewEmail);

            OrganizationContext organizationContext = _stateManager.GetContext();

            ConfirmEmailEmailCommand emailCommand = new ConfirmEmailEmailCommand(command.NewEmail)
            {
                OrganizationFullName = organizationContext.FullName,
                HomeUrl = _urlGenerator.GenerateAbsoluteHomeUrl(),
                LogoUrl = _logoService.GetLogoUrl(),
                TokenLifespan = _userManager.TokenLifespan
            };
            emailCommand.Token = await _userManager.GenerateEmailConfirmationTokenAsync(user.Id);
            emailCommand.ConfirmUrl = _urlGenerator.GenerateConfirmEmailAbsoluteUrl(command.NewEmail, emailCommand.Token);
            await _emailService.SendEmailAsync(emailCommand);

            return CommandResult.Success();
        }
    }
}
