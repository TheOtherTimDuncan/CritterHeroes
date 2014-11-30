using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Domain.Contracts;
using CH.Domain.Contracts.Identity;
using CH.Domain.Contracts.Queries;
using CH.Domain.Identity;
using CH.Website.Models;
using CH.Website.Services.Queries;

namespace CH.Website.Services.QueryHandlers
{
    public class EditProfileViewModelQueryHandler : IQueryHandler<EditProfileQuery, EditProfileModel>
    {
        private IHttpContext _httpContext;
        private IApplicationUserManager _userManager;

        public EditProfileViewModelQueryHandler(IHttpContext httpContext, IApplicationUserManager userManager)
        {
            this._httpContext = httpContext;
            this._userManager = userManager;
        }

        public async Task<EditProfileModel> Retrieve(EditProfileQuery query)
        {
            EditProfileModel model = new EditProfileModel();
            model.ReturnUrl = _httpContext.Request.UrlReferrer.AbsoluteUri;

            IdentityUser user = await _userManager.FindByNameAsync(query.Username);
            model.Username = query.Username;
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.Email = user.Email;

            return model;
        }
    }
}