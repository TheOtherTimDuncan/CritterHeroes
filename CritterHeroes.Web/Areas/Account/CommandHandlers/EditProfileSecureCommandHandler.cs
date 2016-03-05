﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Areas.Common.ActionExtensions;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Data.Models.Identity;
using CritterHeroes.Web.Models.LogEvents;
using Microsoft.AspNet.Identity;

namespace CritterHeroes.Web.Areas.Account.CommandHandlers
{
    public class EditProfileSecureCommandHandler : IAsyncCommandHandler<EditProfileSecureModel>
    {
        private IAppUserManager _userManager;
        private IAppLogger _logger;
        private IHttpUser _httpUser;
        private IStateManager<UserContext> _userContextManager;
        private IUrlGenerator _urlGenerator;
        private IEmailService _emailService;

        public EditProfileSecureCommandHandler(IAppUserManager userManager, IAppLogger logger, IHttpUser httpUser, IStateManager<UserContext> userContextManager, IUrlGenerator urlGenerator, IEmailService emailService)
        {
            this._userManager = userManager;
            this._logger = logger;
            this._httpUser = httpUser;
            this._userContextManager = userContextManager;
            this._urlGenerator = urlGenerator;
            this._emailService = emailService;
        }

        public async Task<CommandResult> ExecuteAsync(EditProfileSecureModel command)
        {
            AppUser user = await _userManager.FindByNameAsync(_httpUser.Username);
            user.Person.NewEmail = command.NewEmail;
            user.EmailConfirmed = false;

            IdentityResult identityResult = await _userManager.UpdateAsync(user);
            if (!identityResult.Succeeded)
            {
                return CommandResult.Failed(identityResult.Errors);
            }

            UserContext userContext = UserContext.FromUser(user);
            _userContextManager.SaveContext(userContext);

            _logger.LogEvent(UserLogEvent.LogAction("Email changed from {OldEmail} to {NewEmail}", user.Email, command.NewEmail));

            ConfirmEmailEmailCommand emailCommand = new ConfirmEmailEmailCommand(command.NewEmail);
            emailCommand.EmailData.TokenLifespan = _userManager.TokenLifespan;
            emailCommand.EmailData.Token = await _userManager.GenerateEmailConfirmationTokenAsync(user.Id);
            emailCommand.EmailData.UrlConfirm = _urlGenerator.GenerateConfirmEmailAbsoluteUrl(command.NewEmail, emailCommand.EmailData.Token);
            await _emailService.SendEmailAsync(emailCommand);

            return CommandResult.Success();
        }
    }
}
