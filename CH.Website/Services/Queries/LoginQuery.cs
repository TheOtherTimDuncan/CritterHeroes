using System;
using System.Collections.Generic;
using CH.Domain.Contracts.Queries;

namespace CH.Website.Services.Queries
{
    public class LoginQuery : IQuery
    {
        public string ReturnUrl
        {
            get;
            set;
        }
    }
}