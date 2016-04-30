using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Features.Account.Models;
using CritterHeroes.Web.Features.Account.Queries;

namespace CritterHeroes.Web.Features.Account.QueryHandlers
{
    public class LoginViewModelQueryHandler : IQueryHandler<LoginQuery, LoginModel>
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
