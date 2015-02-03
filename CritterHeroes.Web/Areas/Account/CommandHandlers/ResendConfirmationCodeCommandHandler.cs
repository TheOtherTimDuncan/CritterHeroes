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
    public class ResendConfirmationCodeCommandHandler : IAsyncCommandHandler<ResendConfirmationCodeModel>
    {
        private IUserLogger _userLogger;
        private IApplicationUserManager _appUserManager;
        private IEmailClient _emailClient;
        private IUrlGenerator _urlGenerator;
        private OrganizationContext _organizationContext;

        public ResendConfirmationCodeCommandHandler(IUserLogger userLogger, IApplicationUserManager userManager, IEmailClient emailClient, IUrlGenerator urlGenerator, OrganizationContext organizationContext)
        {
            this._userLogger = userLogger;
            this._appUserManager = userManager;
            this._emailClient = emailClient;
            this._urlGenerator = urlGenerator;
            this._organizationContext = organizationContext;
        }

        public async Task<CommandResult> ExecuteAsync(ResendConfirmationCodeModel command)
        {
            ModalDialogButton button1 = ModalDialogButton.Button(text: "Try Again", cssClass: ButtonCss.Info, isDismissable: true);
            ModalDialogButton button2 = ModalDialogButton.Link(text: "Continue", cssClass: ButtonCss.Primary, url: _urlGenerator.GenerateSiteUrl<AccountController>(x => x.Login(null)));
            command.ModalDialog = new ModalDialogModel()
            {
                Text = "A new confirmation code has been emailed to you. If you can't find the email, please check your spam folder.",
                Buttons = new ModalDialogButton[] { button1, button2 }
            };

            IdentityUser user = await _appUserManager.FindByEmailAsync(command.EmailAddress);
            if (user == null)
            {
                // Don't disclose if email is valid so ignore invalid emails
                await _userLogger.LogActionAsync(UserActions.ResendConfirmationCodeFailure, command.EmailAddress);
                return CommandResult.Failed();
            }

            EmailMessage emailMessage = new EmailMessage()
            {
                From = _organizationContext.EmailAddress
            };
            emailMessage.To.Add(user.Email);

            if (!user.IsEmailConfirmed)
            {
                // If email isn't confirmed we'll assume that's the confirmation code we need
                emailMessage.Subject = "Confirm Email - " + _organizationContext.FullName;

                string code = await _appUserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                string url = _urlGenerator.GenerateAbsoluteUrl<AccountController>(x => x.ConfirmEmail(user.Id, code));

                EmailBuilder
                    .Begin(emailMessage)
                    .AddParagraph("Please confirm your email address by clicking the link below or visiting " + _organizationContext.FullName + " and copying the code into the provided form. This code will be valid for " + _appUserManager.TokenLifespan.TotalHours.ToString() + " hours.")
                    .AddParagraph("Confirmation Code: " + code)
                    .AddParagraph("<a href=\"" + url + "\">Confirm Email</a>")
                    .End();
            }
            else
            {
                // Otherwise we'll assume it's a reset password confirmation code we need
                emailMessage.Subject = "Reset Password - " + _organizationContext.FullName;

                string code = await _appUserManager.GeneratePasswordResetTokenAsync(user.Id);
                string url = _urlGenerator.GenerateAbsoluteUrl<AccountController>(x => x.ResetPassword(code));

                EmailBuilder
                    .Begin(emailMessage)
                    .AddParagraph("Your password for your account at " + _organizationContext.FullName + " has been reset. To complete resetting your password, click the link below or visit " + _organizationContext.FullName + " and copy the code into the provided form. This code will be valid for " + _appUserManager.TokenLifespan.TotalHours.ToString() + " hours.")
                    .AddParagraph("Confirmation Code: " + code)
                    .AddParagraph("<a href=\"" + url + "\">Reset Password</a>")
                    .End();
            }

            await _emailClient.SendAsync(emailMessage, user.Id);

            await _userLogger.LogActionAsync(UserActions.ResendConfirmationCodeSuccess, user.Email);

            return CommandResult.Success();
        }
    }
}