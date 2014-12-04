using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Domain.Contracts;
using CH.Domain.Contracts.Identity;
using CH.Domain.Contracts.Queries;
using CH.Domain.Identity;
using CH.Domain.Queries;
using CH.Website.Models;

namespace CH.Website.Services.QueryHandlers
{
    public class EditProfileViewModelQueryHandler : IQueryHandler<UserQuery, EditProfileModel>
    {
        private IHttpContext _httpContext;
        private IApplicationUserStore _userStore;

        public EditProfileViewModelQueryHandler(IHttpContext httpContext, IApplicationUserStore userStore)
        {
            this._httpContext = httpContext;
            this._userStore = userStore;
        }

        public async Task<EditProfileModel> Retrieve(UserQuery query)
        {
            EditProfileModel model = new EditProfileModel();
            model.ReturnUrl = _httpContext.Request.UrlReferrer.AbsoluteUri;

            IdentityUser user = await _userStore.FindByNameAsync(query.Username);
            model.Username = query.Username;
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.Email = user.Email;

            return model;
        }
    }
}