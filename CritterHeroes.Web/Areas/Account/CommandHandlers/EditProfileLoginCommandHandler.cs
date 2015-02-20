using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Identity;

namespace CritterHeroes.Web.Areas.Account.CommandHandlers
{
    public class EditProfileLoginCommandHandler : IAsyncCommandHandler<EditProfileLoginModel>
    {
        private IApplicationUserManager _userManager;
        private IHttpUser _httpUser;

        public EditProfileLoginCommandHandler(IApplicationUserManager userManager, IHttpUser httpUser)
        {
            this._userManager = userManager;
            this._httpUser = httpUser;
        }

        public async Task<CommandResult> ExecuteAsync(EditProfileLoginModel command)
        {
            IdentityUser user = await _userManager.FindByIdAsync(_httpUser.UserID);

            bool confirmed = await _userManager.CheckPasswordAsync(user, command.Password);
            if (confirmed)
            {
                return CommandResult.Success();
            };

            return CommandResult.Failed("", "The password you entered was incorrect.");
        }
    }
}