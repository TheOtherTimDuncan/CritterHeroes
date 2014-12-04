using System;
using CH.Domain.Contracts.Queries;

namespace CH.Domain.Queries
{
    public class UserQuery : IQuery
    {
        public string Username
        {
            get;
            set;
        }
    }
}
