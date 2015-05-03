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
        private IOwinContext _owinContext;
        private IApplicationUserStore _userStore;
        private IHttpUser _httpUser;

        public EditProfileViewModelQueryHandler(IOwinContext owinContext, IHttpUser httpUser, IApplicationUserStore userStore)
        {
            this._owinContext = owinContext;
            this._userStore = userStore;
            this._httpUser = httpUser;
        }

        public async Task<EditProfileModel> RetrieveAsync(UserIDQuery query)
        {
            EditProfileModel model = new EditProfileModel();
            model.ReturnUrl = _owinContext.Request.GetReferrer();

            IdentityUser user = await _userStore.FindByIdAsync(_httpUser.UserID);
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