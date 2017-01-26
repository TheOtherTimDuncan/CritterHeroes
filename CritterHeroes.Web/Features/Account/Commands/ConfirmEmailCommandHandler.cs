using System;
using System.Threading.Tasks;
using CritterHeroes.Web.Data.Models.Identity;
using CritterHeroes.Web.Domain.Contracts.Commands;
using CritterHeroes.Web.Domain.Contracts.Events;
using CritterHeroes.Web.Domain.Contracts.Identity;
using CritterHeroes.Web.Features.Account.Models;
using CritterHeroes.Web.Models.LogEvents;
using CritterHeroes.Web.Shared.Commands;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace CritterHeroes.Web.Features.Account.Commands
{
    public class ConfirmEmailCommandHandler : IAsyncCommandHandler<ConfirmEmailModel>
    {
        private IAppEventPublisher _publisher;
        private IAppUserManager _appUserManager;
        private IAuthenticationManager _authenticationManager;

        public ConfirmEmailCommandHandler(IAppEventPublisher publisher, IAppUserManager userManager, IAuthenticationManager authenticationManager)
        {
            this._publisher = publisher;
            this._appUserManager = userManager;
            this._authenticationManager = authenticationManager;
        }

        public async Task<CommandResult> ExecuteAsync(ConfirmEmailModel command)
        {
            AppUser user = await _appUserManager.FindByUnconfirmedEmailAsync(command.Email);

            if (user == null)
            {
                // We don't want to reveal whether or not the email address is valid
                // so if the user isn't found just return failed with no errors

                _publisher.Publish(UserLogEvent.Error("Email confirmation failed for {Email} using {Code} - user not found", command.Email, command.ConfirmationCode));

                return CommandResult.Failed("There was an error confirming your email address. Please try again.");
            }

            IdentityResult identityResult = await _appUserManager.ConfirmEmailAsync(user.Id, command.ConfirmationCode);
            if (identityResult.Succeeded)
            {
                // Update the email address
                user.Email = user.Person.NewEmail;
                await _appUserManager.UpdateAsync(user);

                // In case the user is logged in make sure they are logged out so the new email address is used
                _authenticationManager.SignOut();

                // Let the view know we succeeded
                command.IsSuccess = true;

                _publisher.Publish(UserLogEvent.Action("Email confirmation succeeded for {Email}", command.Email));

                return CommandResult.Success();
            }

            _publisher.Publish(UserLogEvent.Error("Email confirmation failed for {Email} using {Code}", identityResult.Errors, command.Email, command.ConfirmationCode));

            return CommandResult.Failed("There was an error confirming your email address. Please try again.");
        }
    }
}
