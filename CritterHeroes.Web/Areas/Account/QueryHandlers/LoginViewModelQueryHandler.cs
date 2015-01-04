using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Areas.Account.Queries;
using CritterHeroes.Web.Contracts.Queries;

namespace CritterHeroes.Web.Areas.Account.QueryHandlers
{
    public class LoginViewModelQueryHandler : IQueryHandler<LoginQuery, LoginModel>
    {
        public LoginModel Retrieve(LoginQuery query)
        {
            LoginModel model = new LoginModel()
            {
                ReturnUrl = query.ReturnUrl
            };
            return model;
        }
    }
}