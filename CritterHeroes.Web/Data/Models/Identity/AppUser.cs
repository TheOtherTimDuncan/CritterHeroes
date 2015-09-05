using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CritterHeroes.Web.Data.Models.Identity
{
    public class AppUser : IdentityUser<int, AppUserLogin, AppUserRole, AppUserClaim>
    {
        protected AppUser()
        {
        }

        public AppUser(string email)
        {
            this.Person = new Person();
            this.UserName = email;
            this.Email = email;
        }

        public int PersonID
        {
            get;
            private set;
        }

        public virtual Person Person
        {
            get;
            private set;
        }

        public override string UserName
        {
            get
            {
                return base.UserName;
            }
            set
            {
                base.UserName = value;
                base.Email = value;
            }
        }

        public override string Email
        {
            get
            {
                return base.Email;
            }
            set
            {
                base.Email = value;
                base.UserName = value;

                if (Person != null)
                {
                    Person.Email = value;
                }
            }
        }

        public override bool EmailConfirmed
        {
            get
            {
                return base.EmailConfirmed;
            }

            set
            {
                base.EmailConfirmed = value;

                if (Person != null)
                {
                    Person.IsEmailConfirmed = value;
                }
            }
        }
    }
}