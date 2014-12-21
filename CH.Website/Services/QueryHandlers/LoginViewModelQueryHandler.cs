using System;
using System.Collections.Generic;
using System.Linq;
using CH.Domain.Contracts.Queries;
using CH.Website.Models.Account;
using CH.Website.Services.Queries;

namespace CH.Website.Services.QueryHandlers
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