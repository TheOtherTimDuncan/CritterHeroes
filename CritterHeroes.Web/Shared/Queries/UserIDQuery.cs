using System;
using CritterHeroes.Web.Shared.StateManagement;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Features.Account.Models;

namespace CritterHeroes.Web.Shared.Queries
{
    public class UserIDQuery : IAsyncQuery<EditProfileModel>, IAsyncQuery<UserContext>
    {
        public string UserID
        {
            get;
            set;
        }
    }
}
