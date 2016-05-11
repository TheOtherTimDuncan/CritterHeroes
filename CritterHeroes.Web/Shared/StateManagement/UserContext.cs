using System;
using CritterHeroes.Web.Shared.Identity;
using CritterHeroes.Web.Data.Models.Identity;

namespace CritterHeroes.Web.Shared.StateManagement
{
    public class UserContext
    {
        public static UserContext FromUser(AppUser user)
        {
            return new UserContext()
            {
                UserID = user.Id.ToString(),
                DisplayName = user.Person.FirstName + " " + user.Person.LastName
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
