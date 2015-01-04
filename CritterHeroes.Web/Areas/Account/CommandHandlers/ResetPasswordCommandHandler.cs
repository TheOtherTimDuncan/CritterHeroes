using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Areas.Common;
using CritterHeroes.Web.Areas.Home;
using CritterHeroes.Web.Areas.Models.Modal;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Models.Logging;
using Microsoft.AspNet.Identity;

namespace CritterHeroes.Web.Areas.Account.CommandHandlers
{
    public class ResetPasswordCommandHandler : BaseLoginCommandHandler<ResetPasswordModel>
    {
        private IApplicationUserManager _userManager;
        private IUrlGenerator _urlGenerator;

        public ResetPasswordCommandHandler(IApplicationSignInManager signinManager, IUserLogger userLogger, IApplicationUserManager userManager, IUrlGenerator urlGenerator)
            : base(signinManager, userLogger)
        {
            this._userManager = userManager;
            this._urlGenerator = urlGenerator;
        }

        public override async Task<CommandResult> ExecuteAsync(ResetPasswordModel command)
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

            await UserLogger.LogActionAsync(UserActions.ResetPasswordFailure, command.Username, "Code: " + command.Code);
            return CommandResult.Failed("", "There was an error resetting your password. Please try again.");
        }
    }
}