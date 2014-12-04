using System;
using CH.Domain.Identity;

namespace CH.Domain.StateManagement
{
    public class UserContext
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
