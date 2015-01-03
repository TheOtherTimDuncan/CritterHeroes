using System;
using System.Collections.Generic;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Contracts.Queries;

namespace CritterHeroes.Web.Areas.Account.Queries
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