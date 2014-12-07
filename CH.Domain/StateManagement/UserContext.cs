using System;
using CH.Domain.Contracts.Queries;
using CH.Domain.Identity;

namespace CH.Domain.StateManagement
{
    public class UserContext : IQueryResult
    {
        public static UserContext FromUser(IdentityUser user)
        {
            return new UserContext()
            {
                DisplayName = user.FirstName + " " + user.LastName
            };
        }

        public string DisplayName
        {
            get;
            set;
        }
    }
}
