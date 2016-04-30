using System;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Features.Account.Models;

namespace CritterHeroes.Web.Common.Queries
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
