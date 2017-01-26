using System;
using CritterHeroes.Web.Domain.Contracts.Queries;
using CritterHeroes.Web.Features.Account.Models;
using CritterHeroes.Web.Shared.StateManagement;

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
