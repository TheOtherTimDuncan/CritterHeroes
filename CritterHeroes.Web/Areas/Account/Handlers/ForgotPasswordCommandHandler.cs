using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Areas.Admin.DataMaintenance.Handlers;
using CritterHeroes.Web.Areas.Common;
using CritterHeroes.Web.Areas.Models.Modal;
using CritterHeroes.Web.Common.Handlers.Email;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Common.Services.Commands;
using CritterHeroes.Web.Common.Services.Queries;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Models;
using CritterHeroes.Web.Models.Logging;
using TOTD.Utility.ExceptionHelpers;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.Areas.Account.Handlers
{
    public class ForgotPasswordCommandHandler : IAsyncCommandHandler<ForgotPasswordModel>
    {
        private IApplicationUserManager _appUserManager;
        private IEmailClient _emailClient;
        private IUserLogger _userLogger;
        private IAsyncQueryHandler<OrganizationQuery, OrganizationContext> _handlerOrganizationContext;
        private IAppConfiguration _appConfiguration;
        private IUrlGenerator _urlGenerator;

        public ForgotPasswordCommandHandler(IApplicationUserManager userManager, IEmailClient emailClient, IUserLogger userLogger, IAsyncQueryHandler<OrganizationQuery, OrganizationContext> handlerOrganizationContext, IAppConfiguration appConfiguration, IUrlGenerator urlGenerator)
        {
            this._appUserManager = userManager;
            this._emailClient = emailClient;
            this._userLogger = userLogger;
            this._handlerOrganizationContext = handlerOrganizationContext;
            this._appConfiguration = appConfiguration;
            this._urlGenerator = urlGenerator;
        }

        public async Task<CommandResult> ExecuteAsync(ForgotPasswordModel command)
        {
            ThrowIf.Argument.IsNull(command, "command");

            if (command.EmailAddress.IsNullOrWhiteSpace() && command.Username.IsNullOrWhiteSpace())
            {
                return CommandResult.Failed("", "Please enter your email address or your username.");
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

            OrganizationContext organizationContext = await _handlerOrganizationContext.RetrieveAsync(new OrganizationQuery()
            {
                OrganizationID = _appConfiguration.OrganizationID
            });

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
                // so if the user isn't found just return success
                await _userLogger.LogActionAsync(UserActions.ForgotPasswordFailure, null, command);
                return CommandResult.Success();
            }

            string code = await _appUserManager.GeneratePasswordResetTokenAsync(user.Id);
            string url = _urlGenerator.GenerateAbsoluteUrl<AccountController>(x => x.ResetPassword(code));

            EmailMessage emailMessage = new EmailMessage()
            {
                Subject = "Password Reset - " + organizationContext.FullName,
                From = organizationContext.EmailAddress
            };
            emailMessage.To.Add(user.Email);

            EmailBuilder
                .Begin(emailMessage)
                .AddParagraph("Your password for your account at " + organizationContext.FullName + " has been reset. To complete resetting your password, click the link below or visit " + organizationContext.FullName + " and copy the code into the provided form. This code will be valid for " + _appUserManager.TokenLifespan.TotalHours.ToString() + " hours.")
                .AddParagraph("Confirmation Code: " + code)
                .AddParagraph("<a href=\"" + url + "\">Reset Password</a>")
                .End();

            await _emailClient.SendAsync(emailMessage, user.Id);
            await _userLogger.LogActionAsync(UserActions.ForgotPasswordSuccess, user.UserName, command);

            return CommandResult.Success();
        }
    }
}