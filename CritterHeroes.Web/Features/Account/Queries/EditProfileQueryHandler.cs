using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Extensions;
using CritterHeroes.Web.Data.Models.Identity;
using CritterHeroes.Web.Features.Account.Models;
using CritterHeroes.Web.Shared.Queries;

namespace CritterHeroes.Web.Features.Account.Queries
{
    public class EditProfileQueryHandler : IAsyncQueryHandler<UserIDQuery, EditProfileModel>
    {
        private ISqlQueryStorageContext<AppUser> _userStorage;
        private IHttpUser _httpUser;

        public EditProfileQueryHandler(IHttpUser httpUser, ISqlQueryStorageContext<AppUser> userStorageContext)
        {
            this._userStorage = userStorageContext;
            this._httpUser = httpUser;
        }

        public async Task<EditProfileModel> ExecuteAsync(UserIDQuery query)
        {
            EditProfileModel model = new EditProfileModel();

            AppUser user = await _userStorage.Entities.FindByUsernameAsync(_httpUser.Username);
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
