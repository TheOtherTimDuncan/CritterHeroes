﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Domain.Contracts.Commands;
using CH.Domain.Contracts.Identity;
using CH.Domain.Contracts.Logging;
using CH.Domain.Identity;
using CH.Domain.Models.Logging;
using CH.Domain.Services.Commands;
using CH.Website.Controllers;
using CH.Website.Models.Account;
using CH.Website.Models.Modal;
using CH.Website.Utility;
using Microsoft.AspNet.Identity;

namespace CH.Website.Services.CommandHandlers
{
    public class ResetPasswordCommandHandler : BaseLoginCommandHandler<ResetPasswordModel, ModalDialogCommandResult>
    {
        private IApplicationUserManager _userManager;

        public ResetPasswordCommandHandler(IApplicationSignInManager signinManager, IUserLogger userLogger, IApplicationUserManager userManager)
            : base(signinManager, userLogger)
        {
            this._userManager = userManager;
        }

        public override async Task<ModalDialogCommandResult> ExecuteAsync(ResetPasswordModel command)
        {
            IdentityUser identityUser = await _userManager.FindByNameAsync(command.Username);
            if (identityUser != null)
            {
                IdentityResult identityResult = await _userManager.ResetPasswordAsync(identityUser.Id, command.Code, command.Password);
                if (identityResult.Succeeded)
                {
                    await UserLogger.LogActionAsync(UserActions.ResetPasswordSuccess, command.Username);

                    CommandResult commandResult = await Login(command);
                    if (commandResult.Succeeded)
                    {
                        ModalDialogCommandResult result = ModalDialogCommandResult.Success();
                        result.ModalDialog = new ModalDialogModel()
                        {
                            Text = "Your password has been successfully reset.",
                            Buttons = new ModalDialogButton[] { ModalDialogButton.Link("Continue", ButtonCss.Primary, command.UrlGenerator.GenerateSiteUrl<HomeController>(x => x.Index())) }
                        };
                        return result;
                    }
                }
            }

            await UserLogger.LogActionAsync(UserActions.ResetPasswordFailure, command.Username, "Code: " + command.Code);
            return ModalDialogCommandResult.Failed("", "There was an error resetting your password. Please try again.");
        }
    }
}