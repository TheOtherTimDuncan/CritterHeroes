using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Domain.Contracts.Queries;
using CH.Website.Models;
using CH.Website.Services.Queries;

namespace CH.Website.Services.QueryHandlers
{
    public class LoginViewModelQueryHandler : IQueryHandler<LoginQuery, LoginModel>
    {
        public Task<LoginModel> Retrieve(LoginQuery query)
        {
            LoginModel model = new LoginModel()
            {
                ReturnUrl = query.ReturnUrl
            };
            return Task.FromResult(model);
        }
    }
}