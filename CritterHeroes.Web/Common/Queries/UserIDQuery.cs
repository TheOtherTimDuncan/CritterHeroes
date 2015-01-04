using System;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.Queries;

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
