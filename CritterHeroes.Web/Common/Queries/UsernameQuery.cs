using System;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Contracts.Queries;

namespace CritterHeroes.Web.Common.Queries
{
    public class UsernameQuery : IAsyncQuery<CheckUsernameResult>
    {
        public string Username
        {
            get;
            set;
        }
    }
}
