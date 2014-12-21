using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Domain.Contracts.Commands;
using CH.Domain.Contracts.Email;
using CH.Domain.Contracts.Identity;
using CH.Domain.Contracts.Logging;
using CH.Domain.Handlers.Emails;
using CH.Domain.Identity;
using CH.Domain.Models;
using CH.Domain.Models.Logging;
using CH.Domain.Services.Commands;
using CH.Website.Controllers;
using CH.Website.Models.Account;
using CH.Website.Models.Modal;
using CH.Website.Services.Commands;
using CH.Website.Utility;
using TOTD.Utility.ExceptionHelpers;
using TOTD.Utility.StringHelpers;

namespace CH.Website.Services.CommandHandlers
{
    public class ForgotPasswordCommandHandler : IAsyncCommandHandler<ForgotPasswordCommand, ModalDialogCommandResult>
    {
        private IApplicationUserManager _appUserManager;
        private IEmailClient _emailClient;
        private IUserLogger _userLogger;

        public ForgotPasswordCommandHandler(IApplicationUserManager userManager, IEmailClient emailClient, IUserLogger userLogger)
        {
            this._appUserManager = userManager;
            this._emailClient = emailClient;
            this._userLogger = userLogger;
        }

        public async Task<ModalDialogCommandResult> ExecuteAsync(ForgotPasswordCommand command)
        {
            ThrowIf.Argument.IsNull(command, "command");

            if (command.EmailAddress.IsNullOrWhiteSpace() && command.Username.IsNullOrWhiteSpace())
            {
                return ModalDialogCommandResult.Failed("", "Please enter your email address or your username.");
            }

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
            ModalDialogButton button2 = ModalDialogButton.Link(text: "Continue", cssClass: ButtonCss.Primary, url: command.UrlGenerator.GenerateSiteUrl<AccountController>(x => x.Login(null)));
            ModalDialogModel dialog = new ModalDialogModel()
            {
                Text = "Instructions for resetting your password have been emailed to you. Please check your email and follow the provided instructions to complete resetting your password. If you can't find the email, please check your spam folder.",
                Buttons = new ModalDialogButton[] { button1, button2 }
            };

            if (user == null)
            {
                // We don't want to reveal whether or not the username or email address are valid
                // so if the user isn't found just return success
                await _userLogger.LogActionAsync(UserActions.ForgotPasswordFailure, null, command);
                return ModalDialogCommandResult.Success(dialog);
            }

            string code = await _appUserManager.GeneratePasswordResetTokenAsync(user.Id);
            string url = command.UrlGenerator.GenerateAbsoluteUrl<AccountController>(x => x.ResetPassword(code));

            EmailMessage emailMessage = new EmailMessage()
            {
                Subject = "Password Reset - " + command.OrganizationFullName,
                From = command.OrganizationEmailAddress
            };
            emailMessage.To.Add(user.Email);

            EmailBuilder
                .Begin(emailMessage)
                .AddParagraph("Your password for your account at " + command.OrganizationFullName + " has been reset. To complete resetting your password, click the link below or visit " + command.OrganizationFullName + " and copy the code into the provided form. This code will be valid for " + _appUserManager.TokenLifespan.TotalHours.ToString() + " hours.")
                .AddParagraph("Confirmation Code: " + code)
                .AddParagraph("<a href=\"" + url + "\">Reset Password</a>")
                .End();

            await _emailClient.SendAsync(emailMessage, user.Id);
            await _userLogger.LogActionAsync(UserActions.ForgotPasswordSuccess, user.UserName, command);

            return ModalDialogCommandResult.Success(dialog);
        }
    }
}