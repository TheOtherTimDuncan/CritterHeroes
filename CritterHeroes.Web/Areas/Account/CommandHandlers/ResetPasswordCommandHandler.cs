using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.Email;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Data.Models.Identity;
using CritterHeroes.Web.Models;
using CritterHeroes.Web.Models.Logging;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace CritterHeroes.Web.Areas.Account.CommandHandlers
{
    public class ResetPasswordCommandHandler : IAsyncCommandHandler<ResetPasswordModel>
    {
        private IAppUserManager _userManager;
        private IAppSignInManager _signinManager;
        private IUserLogger _userLogger;
        private IEmailClient _emailClient;
        private IStateManager<OrganizationContext> _organizationStateManager;

        public ResetPasswordCommandHandler(IUserLogger userLogger, IAppSignInManager signinManager, IAppUserManager userManager, IEmailClient emailClient, IStateManager<OrganizationContext> organizationStateManager)
        {
            this._userManager = userManager;
            this._signinManager = signinManager;
            this._userLogger = userLogger;
            this._emailClient = emailClient;
            this._organizationStateManager = organizationStateManager;
        }

        public async Task<CommandResult> ExecuteAsync(ResetPasswordModel command)
        {
            AppUser identityUser = await _userManager.FindByEmailAsync(command.Email);
            if (identityUser != null)
            {
                IdentityResult identityResult = await _userManager.ResetPasswordAsync(identityUser.Id, command.Code, command.Password);
                if (identityResult.Succeeded)
                {
                    SignInStatus loginResult = await _signinManager.PasswordSignInAsync(command.Email, command.Password);
                    if (loginResult == SignInStatus.Success)
                    {
                        OrganizationContext orgcontext = _organizationStateManager.GetContext();
                        EmailMessage emailMessage = new EmailMessage()
                        {
                            Subject = "Admin Notification - " + orgcontext.FullName,
                            From = orgcontext.EmailAddress
                        };
                        emailMessage.To.Add(identityUser.Email);

                        EmailBuilder
                            .Begin(emailMessage)
                            .AddParagraph("This is a notification that your password has been successfuly reset.")
                            .End();

                        await _emailClient.SendAsync(emailMessage);
                        await _userLogger.LogActionAsync(UserActions.ResetPasswordSuccess, command.Email);

                        // Let the view know we succeeded
                        command.IsSuccess = true;

                        return CommandResult.Success();
                    }
                }
            }

            await _userLogger.LogActionAsync(UserActions.ResetPasswordFailure, command.Email, "Code: " + command.Code);

            return CommandResult.Failed("There was an error resetting your password. Please try again.");
        }
    }
}