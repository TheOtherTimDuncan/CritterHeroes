using System;
using System.Collections.Generic;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Features.Account.Models;

namespace CritterHeroes.Web.Features.Account.Queries
{
    public class LoginQuery : IQuery<LoginModel>
    {
        public string ReturnUrl
        {
            get;
            set;
        }
    }
}
