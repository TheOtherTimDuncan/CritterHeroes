using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Areas.Common;
using CritterHeroes.Web.Areas.Models.Modal;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.Handlers.Email;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Models;
using CritterHeroes.Web.Models.Logging;

namespace CritterHeroes.Web.Areas.Account.CommandHandlers
{
    public class ForgotUsernameCommandHandler : IAsyncCommandHandler<ForgotUsernameModel>
    {
        private IUserLogger _userLogger;
        private IApplicationUserManager _appUserManager;
        private IEmailClient _emailClient;
        private IUrlGenerator _urlGenerator;
        private OrganizationContext _organizationContext;

        public ForgotUsernameCommandHandler(IUserLogger userLogger, IApplicationUserManager userManager, IEmailClient emailClient, IUrlGenerator urlGenerator, OrganizationContext organizationContext)
        {
            this._userLogger = userLogger;
            this._appUserManager = userManager;
            this._emailClient = emailClient;
            this._urlGenerator = urlGenerator;
            this._organizationContext = organizationContext;
        }

        public async Task<CommandResult> ExecuteAsync(ForgotUsernameModel command)
        {
            ModalDialogButton button1 = ModalDialogButton.Button(text: "Try Again", cssClass: ButtonCss.Info, isDismissable: true);
            ModalDialogButton button2 = ModalDialogButton.Link(text: "Continue", cssClass: ButtonCss.Primary, url: _urlGenerator.GenerateSiteUrl<AccountController>(x => x.Login(null)));
            command.ModalDialog = new ModalDialogModel()
            {
                Text = "Your username has been emailed to you. If you can't find the email, please check your spam folder.",
                Buttons = new ModalDialogButton[] { button1, button2 }
            };

            IdentityUser user = await _appUserManager.FindByEmailAsync(command.EmailAddress);
            if (user == null)
            {
                // Don't disclose if email is valid so ignore invalid emails
                await _userLogger.LogActionAsync(UserActions.ForgotUsernameFailure, null, command.EmailAddress);
                return CommandResult.Failed();
            }

            EmailMessage emailMessage = new EmailMessage()
            {
                Subject = "Forgot Username - " + _organizationContext.FullName,
                From = _organizationContext.EmailAddress
            };
            emailMessage.To.Add(user.Email);

            EmailBuilder
                .Begin(emailMessage)
                .AddParagraph("Your username for your account at " + _organizationContext.FullName + " is " + user.UserName + ".")
                .End();
            await _emailClient.SendAsync(emailMessage, user.Id);

            await _userLogger.LogActionAsync(UserActions.ForgotUsernameSuccess, user.UserName);

            return CommandResult.Success();
        }
    }
}