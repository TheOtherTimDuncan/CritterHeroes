using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Domain.Contracts;
using CH.Domain.Contracts.Identity;
using CH.Domain.Contracts.Queries;
using CH.Domain.Identity;
using CH.Domain.Services.Queries;
using CH.Website.Models.Account;

namespace CH.Website.Services.QueryHandlers
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