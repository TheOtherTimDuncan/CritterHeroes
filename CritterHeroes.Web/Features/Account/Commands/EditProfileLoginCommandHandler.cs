using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Data.Models.Identity;
using CritterHeroes.Web.Domain.Contracts;
using CritterHeroes.Web.Domain.Contracts.Commands;
using CritterHeroes.Web.Domain.Contracts.Identity;
using CritterHeroes.Web.Features.Account.Models;
using CritterHeroes.Web.Shared.Commands;

namespace CritterHeroes.Web.Features.Account.Commands
{
    public class EditProfileLoginCommandHandler : IAsyncCommandHandler<EditProfileLoginModel>
    {
        private IAppUserManager _userManager;
        private IHttpUser _httpUser;

        public EditProfileLoginCommandHandler(IAppUserManager userManager, IHttpUser httpUser)
        {
            this._userManager = userManager;
            this._httpUser = httpUser;
        }

        public async Task<CommandResult> ExecuteAsync(EditProfileLoginModel command)
        {
            AppUser user = await _userManager.FindByNameAsync(_httpUser.Username);

            bool confirmed = await _userManager.CheckPasswordAsync(user, command.Password);
            if (confirmed)
            {
                return CommandResult.Success();
            };

            return CommandResult.Failed("The password you entered was incorrect.");
        }
    }
}
