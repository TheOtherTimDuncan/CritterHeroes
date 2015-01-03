using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Areas.Admin.DataMaintenance.Handlers;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Common.Services.Queries;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Queries;

namespace CritterHeroes.Web.Areas.Account.Handlers
{
    public class EditProfileViewModelQueryHandler : IAsyncQueryHandler<UserIDQuery, EditProfileModel>
    {
        private IHttpContext _httpContext;
        private IApplicationUserStore _userStore;

        public EditProfileViewModelQueryHandler(IHttpContext httpContext, IApplicationUserStore userStore)
        {
            this._httpContext = httpContext;
            this._userStore = userStore;
        }

        public async Task<EditProfileModel> RetrieveAsync(UserIDQuery query)
        {
            EditProfileModel model = new EditProfileModel();
            model.ReturnUrl = _httpContext.Request.UrlReferrer.AbsoluteUri;

            IdentityUser user = await _userStore.FindByIdAsync(query.UserID);
            model.Username = user.UserName;
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.Email = user.Email;

            return model;
        }
    }
}