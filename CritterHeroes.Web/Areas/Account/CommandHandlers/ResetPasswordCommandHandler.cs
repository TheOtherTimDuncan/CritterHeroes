using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Areas.Common;
using CritterHeroes.Web.Areas.Home;
using CritterHeroes.Web.Areas.Models.Modal;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.Email;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Models;
using CritterHeroes.Web.Models.Logging;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace CritterHeroes.Web.Areas.Account.CommandHandlers
{
    public class ResetPasswordCommandHandler : IAsyncCommandHandler<ResetPasswordModel>
    {
        private IApplicationUserManager _userManager;
        private IApplicationSignInManager _signinManager;
        private IUrlGenerator _urlGenerator;
        private IUserLogger _userLogger;
        private IEmailClient _emailClient;
        private IStateManager<OrganizationContext> _organizationStateManager;

        public ResetPasswordCommandHandler(IUserLogger userLogger, IApplicationSignInManager signinManager, IApplicationUserManager userManager, IUrlGenerator urlGenerator, IEmailClient emailClient, IStateManager<OrganizationContext> organizationStateManager)
        {
            this._userManager = userManager;
            this._signinManager = signinManager;
            this._urlGenerator = urlGenerator;
            this._userLogger = userLogger;
            this._emailClient = emailClient;
            this._organizationStateManager = organizationStateManager;
        }

        public async Task<CommandResult> ExecuteAsync(ResetPasswordModel command)
        {
            IdentityUser identityUser = await _userManager.FindByEmailAsync(command.Email);
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

                        CommandResult result = CommandResult.Success();
                        command.ModalDialog = new ModalDialogModel()
                        {
                            Text = "Your password has been successfully reset.",
                            Buttons = new ModalDialogButton[] { ModalDialogButton.Link("Continue", ButtonCss.Primary, _urlGenerator.GenerateSiteUrl<HomeController>(x => x.Index())) }
                        };
                        return result;
                    }
                }
            }

            await _userLogger.LogActionAsync(UserActions.ResetPasswordFailure, command.Email, "Code: " + command.Code);

            return CommandResult.Failed("There was an error resetting your password. Please try again.");
        }
    }
}