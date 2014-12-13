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
using CH.Website.Services.Commands;
using CH.Website.Utility;
using TOTD.Utility.ExceptionHelpers;
using TOTD.Utility.StringHelpers;

namespace CH.Website.Services.CommandHandlers
{
    public class ForgotPasswordCommandHandler : ICommandHandler<ForgotPasswordCommand>
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

        public async Task<CommandResult> Execute(ForgotPasswordCommand command)
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

            if (user == null)
            {
                // We don't want to reveal whether or not the username or email address are valid
                // so if the user isn't found just return success
                await _userLogger.LogActionAsync(UserActions.ForgotPasswordFailure, null, command);
                return CommandResult.Success();
            }

            ResetPasswordModel resetModel = new ResetPasswordModel()
            {
                Code = await _appUserManager.GeneratePasswordResetTokenAsync(user.Id)
            };
            string url = command.UrlGenerator.GenerateAbsoluteUrl<AccountController>(x => x.ResetPassword(resetModel));

            EmailMessage emailMessage = new EmailMessage()
            {
                Subject = "Password Reset - " + command.OrganizationFullName
            };
            emailMessage.To.Add(user.Email);

            EmailBuilder
                .Begin(emailMessage)
                .AddParagraph("Your password for your account at " + command.OrganizationFullName + " has been reset. To complete resetting your password, click the link below or visit " + command.OrganizationFullName + " and copy the code into the provided form. This code will be valid for " + _appUserManager.TokenLifespan.TotalHours.ToString() + " hours.")
                .AddParagraph("Confirmation Code: " + resetModel.Code)
                .AddParagraph("<a href=\"" + url + "\">Reset Password</a>")
                .End();

            await _emailClient.SendAsync(emailMessage);
            await _userLogger.LogActionAsync(UserActions.ForgotPasswordSuccess, user.UserName, command);

            return CommandResult.Success();
        }
    }
}