using System;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Data.Models.Identity;

namespace CritterHeroes.Web.Common.StateManagement
{
    public class UserContext
    {
        public static UserContext FromUser(AppUser user)
        {
            return new UserContext()
            {
                UserID = user.Id.ToString(),
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
