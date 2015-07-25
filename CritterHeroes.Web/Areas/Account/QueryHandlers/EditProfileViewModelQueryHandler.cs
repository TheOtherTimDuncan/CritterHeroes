using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Common.OwinExtensions;
using CritterHeroes.Web.Common.Queries;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Queries;
using Microsoft.Owin;

namespace CritterHeroes.Web.Areas.Account.QueryHandlers
{
    public class EditProfileViewModelQueryHandler : IAsyncQueryHandler<UserIDQuery, EditProfileModel>
    {
        private IAppUserStore _userStore;
        private IHttpUser _httpUser;

        public EditProfileViewModelQueryHandler(IHttpUser httpUser, IAppUserStore userStore)
        {
            this._userStore = userStore;
            this._httpUser = httpUser;
        }

        public async Task<EditProfileModel> RetrieveAsync(UserIDQuery query)
        {
            EditProfileModel model = new EditProfileModel();

            AppUser user = await _userStore.FindByIdAsync(_httpUser.UserID);
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.Email = user.Email;

            if (!user.IsEmailConfirmed)
            {
                model.UnconfirmedEmail = user.NewEmail;
            }

            return model;
        }
    }
}