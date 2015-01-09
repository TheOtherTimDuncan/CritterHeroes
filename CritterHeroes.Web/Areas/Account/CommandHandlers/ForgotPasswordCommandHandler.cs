using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.Commands;
using CritterHeroes.Web.Areas.Common;
using CritterHeroes.Web.Areas.Models.Modal;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.Handlers.Email;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Common.Notifications;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Notifications;
using CritterHeroes.Web.Models;
using CritterHeroes.Web.Models.Logging;
using TOTD.Utility.ExceptionHelpers;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.Areas.Account.CommandHandlers
{
    public class ForgotPasswordCommandHandler : IAsyncCommandHandler<ForgotPasswordCommand>
    {
        private INotificationPublisher _publisher;
        private IApplicationUserManager _appUserManager;
        private IEmailClient _emailClient;
        private IUrlGenerator _urlGenerator;

        public ForgotPasswordCommandHandler(INotificationPublisher publisher, IApplicationUserManager userManager, IEmailClient emailClient, IUrlGenerator urlGenerator)
        {
            this._publisher = publisher;
            this._appUserManager = userManager;
            this._emailClient = emailClient;
            this._urlGenerator = urlGenerator;
        }

        public async Task<CommandResult> ExecuteAsync(ForgotPasswordCommand command)
        {
            ThrowIf.Argument.IsNull(command, "command");

            if (command.Model.EmailAddress.IsNullOrWhiteSpace() && command.Model.Username.IsNullOrWhiteSpace())
            {
                return CommandResult.Failed("", "Please enter your email address or your username.");
            }

            IdentityUser user;
            string notificationUsername;
            if (!command.Model.EmailAddress.IsNullOrWhiteSpace())
            {
                notificationUsername = command.Model.EmailAddress;
                user = await _appUserManager.FindByEmailAsync(command.Model.EmailAddress);
            }
            else
            {
                notificationUsername = command.Model.Username;
                user = await _appUserManager.FindByNameAsync(command.Model.Username);
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
                await _publisher.PublishAsync(new UserActionNotification(UserActions.ForgotPasswordFailure, null, command.Model));
                return CommandResult.Failed();
            }

            command.Model.Username = user.UserName;

            string code = await _appUserManager.GeneratePasswordResetTokenAsync(user.Id);
            string url = _urlGenerator.GenerateAbsoluteUrl<AccountController>(x => x.ResetPassword(code));

            EmailMessage emailMessage = new EmailMessage()
            {
                Subject = "Password Reset - " + command.OrganizationContext.FullName,
                From = command.OrganizationContext.EmailAddress
            };
            emailMessage.To.Add(user.Email);

            EmailBuilder
                .Begin(emailMessage)
                .AddParagraph("Your password for your account at " + command.OrganizationContext.FullName + " has been reset. To complete resetting your password, click the link below or visit " + command.OrganizationContext.FullName + " and copy the code into the provided form. This code will be valid for " + _appUserManager.TokenLifespan.TotalHours.ToString() + " hours.")
                .AddParagraph("Confirmation Code: " + code)
                .AddParagraph("<a href=\"" + url + "\">Reset Password</a>")
                .End();

            await _emailClient.SendAsync(emailMessage, user.Id);
            await _publisher.PublishAsync(new UserActionNotification(UserActions.ForgotPasswordSuccess, notificationUsername));
            return CommandResult.Success();
        }
    }
}