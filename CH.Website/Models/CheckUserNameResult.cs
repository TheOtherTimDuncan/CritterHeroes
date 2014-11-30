using System;
using System.Collections.Generic;
using CH.Domain.Contracts.Queries;

namespace CH.Website.Models
{
    public class CheckUsernameResult : IQueryResult
    {
        public bool UserExists
        {
            get;
            set;
        }
    }
}