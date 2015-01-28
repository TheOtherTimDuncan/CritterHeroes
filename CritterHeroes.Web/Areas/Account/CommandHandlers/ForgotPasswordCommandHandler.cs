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
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.Areas.Account.CommandHandlers
{
    public class ForgotPasswordCommandHandler : IAsyncCommandHandler<ForgotPasswordModel>
    {
        private IUserLogger _userLogger;
        private IApplicationUserManager _appUserManager;
        private IEmailClient _emailClient;
        private IUrlGenerator _urlGenerator;
        private OrganizationContext _organizationContext;

        public ForgotPasswordCommandHandler(IUserLogger userLogger, IApplicationUserManager userManager, IEmailClient emailClient, IUrlGenerator urlGenerator, OrganizationContext organizationContext)
        {
            this._userLogger = userLogger;
            this._appUserManager = userManager;
            this._emailClient = emailClient;
            this._urlGenerator = urlGenerator;
            this._organizationContext = organizationContext;
        }

        public async Task<CommandResult> ExecuteAsync(ForgotPasswordModel command)
        {
            IdentityUser user;
            if (!command.EmailAddress.IsNullOrWhiteSpace())
            {
                user = await _appUserManager.FindByEmailAsync(command.EmailAddress);
            }
            else
            {
                user = await _appUserManager.FindByNameAsync(command.Username);
            }

            ModalDialogButton button1 = ModalDialogButton.Button(text: "Try Again", cssClass: ButtonCss.Info, isDismissable: true);
            ModalDialogButton button2 = ModalDialogButton.Link(text: "Continue", cssClass: ButtonCss.Primary, url: _urlGenerator.GenerateSiteUrl<AccountController>(x => x.Login(null)));
            command.ModalDialog = new ModalDialogModel()
            {
                Text = "Instructions for resetting your password have been emailed to you. Please check your email and follow the provided instructions to complete resetting your password. If you can't find the email, please check your spam folder.",
                Buttons = new ModalDialogButton[] { button1, button2 }
            };

            if (user == null)
            {
                // We don't want to reveal whether or not the username or email address are valid
                // so if the user isn't found just return failed with no errors
                await _userLogger.LogActionAsync(UserActions.ForgotPasswordFailure, null, command);
                return CommandResult.Failed();
            }

            string code = await _appUserManager.GeneratePasswordResetTokenAsync(user.Id);
            string url = _urlGenerator.GenerateAbsoluteUrl<AccountController>(x => x.ResetPassword(code));

            EmailMessage emailMessage = new EmailMessage()
            {
                Subject = "Password Reset - " + _organizationContext.FullName,
                From = _organizationContext.EmailAddress
            };
            emailMessage.To.Add(user.Email);

            EmailBuilder
                .Begin(emailMessage)
                .AddParagraph("Your password for your account at " + _organizationContext.FullName + " has been reset. To complete resetting your password, click the link below or visit " + _organizationContext.FullName + " and copy the code into the provided form. This code will be valid for " + _appUserManager.TokenLifespan.TotalHours.ToString() + " hours.")
                .AddParagraph("Confirmation Code: " + code)
                .AddParagraph("<a href=\"" + url + "\">Reset Password</a>")
                .End();

            await _emailClient.SendAsync(emailMessage, user.Id);
            await _userLogger.LogActionAsync(UserActions.ForgotPasswordSuccess, user.UserName);
            return CommandResult.Success();
        }
    }
}