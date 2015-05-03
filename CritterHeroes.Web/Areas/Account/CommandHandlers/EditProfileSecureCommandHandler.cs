using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Areas.Home;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Models.Logging;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace CritterHeroes.Web.Areas.Account.CommandHandlers
{
    public class EditProfileSecureCommandHandler : IAsyncCommandHandler<EditProfileSecureModel>
    {
        private IApplicationUserManager _userManager;
        private IUserLogger _userLogger;
        private IHttpUser _httpUser;
        private IStateManager<UserContext> _userContextManager;
        private IUrlGenerator _urlGenerator;
        private IEmailService _emailService;

        public EditProfileSecureCommandHandler(IApplicationUserManager userManager, IUserLogger userLogger, IHttpUser httpUser, IStateManager<UserContext> userContextManager, IUrlGenerator urlGenerator, IEmailService emailService)
        {
            this._userManager = userManager;
            this._userLogger = userLogger;
            this._httpUser = httpUser;
            this._userContextManager = userContextManager;
            this._urlGenerator = urlGenerator;
            this._emailService = emailService;
        }

        public async Task<CommandResult> ExecuteAsync(EditProfileSecureModel command)
        {
            IdentityUser user = await _userManager.FindByIdAsync(_httpUser.UserID);
            user.NewEmail = command.NewEmail;
            user.IsEmailConfirmed = false;

            IdentityResult identityResult = await _userManager.UpdateAsync(user);
            if (!identityResult.Succeeded)
            {
                return CommandResult.Failed(identityResult.Errors);
            }

            UserContext userContext = UserContext.FromUser(user);
            _userContextManager.SaveContext(userContext);

            await _userLogger.LogActionAsync(UserActions.EmailChanged, user.Email, "New email: " + command.NewEmail);

            ConfirmEmailCommand emailCommand = new ConfirmEmailCommand(command.NewEmail)
            {
                HomeUrl = _urlGenerator.GenerateAbsoluteUrl<HomeController>(x => x.Index()),
                TokenLifespan = _userManager.TokenLifespan
            };
            emailCommand.Token = await _userManager.GenerateEmailConfirmationTokenAsync(user.Id);
            emailCommand.Url = _urlGenerator.GenerateAbsoluteUrl<AccountController>(x => x.ConfirmEmail(command.NewEmail, emailCommand.Token));
            await _emailService.SendEmailAsync(emailCommand);

            return CommandResult.Success();
        }
    }
}