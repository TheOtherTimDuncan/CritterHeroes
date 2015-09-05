using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Common.Queries;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Extensions;
using CritterHeroes.Web.Data.Models.Identity;

namespace CritterHeroes.Web.Areas.Account.QueryHandlers
{
    public class EditProfileViewModelQueryHandler : IAsyncQueryHandler<UserIDQuery, EditProfileModel>
    {
        private ISqlStorageContext<AppUser> _userStorageContext;
        private IHttpUser _httpUser;

        public EditProfileViewModelQueryHandler(IHttpUser httpUser, ISqlStorageContext<AppUser> userStorageContext)
        {
            this._userStorageContext = userStorageContext;
            this._httpUser = httpUser;
        }

        public async Task<EditProfileModel> RetrieveAsync(UserIDQuery query)
        {
            EditProfileModel model = new EditProfileModel();

            AppUser user = await _userStorageContext.Entities.FindByUsernameAsync(_httpUser.Username);
            model.FirstName = user.Person.FirstName;
            model.LastName = user.Person.LastName;
            model.Email = user.Email;

            if (!user.EmailConfirmed)
            {
                model.UnconfirmedEmail = user.Person.NewEmail;
            }

            return model;
        }
    }
}
