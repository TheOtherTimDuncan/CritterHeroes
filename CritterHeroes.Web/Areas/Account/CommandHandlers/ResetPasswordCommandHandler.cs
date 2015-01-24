﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Areas.Common;
using CritterHeroes.Web.Areas.Home;
using CritterHeroes.Web.Areas.Models.Modal;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.Handlers.Email;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Models;
using CritterHeroes.Web.Models.Logging;
using Microsoft.AspNet.Identity;

namespace CritterHeroes.Web.Areas.Account.CommandHandlers
{
    public class ResetPasswordCommandHandler : BaseLoginCommandHandler<ResetPasswordModel>
    {
        private IApplicationUserManager _userManager;
        private IUrlGenerator _urlGenerator;
        private IUserLogger _userLogger;
        private IEmailClient _emailClient;
        private OrganizationContext _organizationContext;

        public ResetPasswordCommandHandler(IUserLogger userLogger, IApplicationSignInManager signinManager, IApplicationUserManager userManager, IUrlGenerator urlGenerator, IEmailClient emailClient, OrganizationContext organizationContext)
            : base(userLogger, signinManager)
        {
            this._userManager = userManager;
            this._urlGenerator = urlGenerator;
            this._userLogger = userLogger;
            this._emailClient = emailClient;
            this._organizationContext = organizationContext;
        }

        public override async Task<CommandResult> ExecuteAsync(ResetPasswordModel command)
        {
            IdentityUser identityUser = await _userManager.FindByNameAsync(command.Username);
            if (identityUser != null)
            {
                IdentityResult identityResult = await _userManager.ResetPasswordAsync(identityUser.Id, command.Code, command.Password);
                if (identityResult.Succeeded)
                {
                    CommandResult commandResult = await Login(command);
                    if (commandResult.Succeeded)
                    {
                        EmailMessage emailMessage = new EmailMessage()
                        {
                            Subject = "Admin Notification - " + _organizationContext.FullName,
                            From = _organizationContext.EmailAddress
                        };
                        emailMessage.To.Add(identityUser.Email);

                        EmailBuilder
                            .Begin(emailMessage)
                            .AddParagraph("This is a notification that your password has been successfuly reset.")
                            .End();

                        await _emailClient.SendAsync(emailMessage);
                        await _userLogger.LogActionAsync(UserActions.ResetPasswordSuccess, command.Username);

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

            await _userLogger.LogActionAsync(UserActions.ResetPasswordFailure, command.Username, "Code: " + command.Code);

            return CommandResult.Failed("", "There was an error resetting your password. Please try again.");
        }
    }
}