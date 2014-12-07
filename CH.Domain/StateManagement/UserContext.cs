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
                UserID = user.Id,
                DisplayName = user.FirstName + " " + user.LastName
            };
        }

        public string UserID
        {
            get;
            set;
        }

        public string DisplayName
        {
            get;
            set;
        }
    }
}
