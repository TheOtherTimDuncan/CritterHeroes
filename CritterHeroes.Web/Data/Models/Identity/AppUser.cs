using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CritterHeroes.Web.Data.Models.Identity
{
    public class AppUser : IdentityUser<int, AppUserLogin, AppUserRole, AppUserClaim>, IUser<int>
    {
        protected AppUser()
        {
        }

        public AppUser(string email)
        {
            this.UserName = email;
            this.Email = email;
        }

        public string FirstName
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }

        public string NewEmail
        {
            get;
            set;
        }
    }
}