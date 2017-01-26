using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Domain.Contracts.Queries;
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

    public class LoginQueryHandler : IQueryHandler<LoginQuery, LoginModel>
    {
        public LoginModel Execute(LoginQuery query)
        {
            LoginModel model = new LoginModel()
            {
                ReturnUrl = query.ReturnUrl
            };
            return model;
        }
    }
}
